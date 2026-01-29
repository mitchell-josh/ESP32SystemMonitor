#include "controllers/ui_controllers.h"

#include <Arduino_GFX_Library.h>
#include <ArduinoJson.h>
#include <lvgl.h>

// External C linkage for SquareLine Studio generated UI files
// This allows C++ to access variables and functions defined in the UI C files
extern "C" {
    #include "ui/ui.h"
    #include "ui/screens/ui_Home.h"
}

/* Hardware & Buffer Setup 
 * Configures the SPI bus and the specific ST7789 display controller
 */
Arduino_DataBus *bus = new Arduino_ESP32SPI(41, 42, 40, 45, -1);
Arduino_GFX *gfx = new Arduino_ST7789(bus, 39, 1, true);

static const uint32_t screenWidth  = 320;
static const uint32_t screenHeight = 240;

// LVGL Drawing Buffers: Used to render pixels before sending to the display
static lv_disp_draw_buf_t draw_buf;
static lv_color_t buf[screenWidth * 10]; // buffer 10 lines of screen

// Track ui initialisation state
bool ui_initialised = false;

/* Function prototypes */
void handle_serial_input();
void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p);

/**
 * @brief Accumulates serial data until a complete JSON object is detected.
 * Uses a "Brace Counting" algorithm to ensure we don't try to parse 
 * incomplete data packets.
 */
void handle_serial_input() {
    static String incomingBuffer = "";
    static int braceCount = 0;

    while (Serial.available()) {
        char c = (char)Serial.read();
        incomingBuffer += c;

        // Count braces to find the start and end of a JSON object
        if (c == '{') braceCount++;
        if (c == '}') braceCount--;

        // If we have at least one brace and they are now balanced
        if (braceCount == 0 && incomingBuffer.indexOf('{') != -1) {
            incomingBuffer.trim();

            // Pass verified string to the UI controller
            update_ui(incomingBuffer.c_str());
            
            // Reset for next full object
            incomingBuffer = "";
        }
        
        // Reset if buffer limit exceeded
        if (incomingBuffer.length() > 1024) {
            incomingBuffer = "";
            braceCount = 0;
        }
    }
}

/**
 * @brief LVGL Flush Callback: Transfers the rendered internal buffer to the LCD.
 * This is the bridge between the LVGL graphics library and the Arduino_GFX driver.
 */
void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p) {
    uint32_t w = (area->x2 - area->x1 + 1);
    uint32_t h = (area->y2 - area->y1 + 1);

    // Push pixels to display
    gfx->draw16bitBeRGBBitmap(area->x1, area->y1, (uint16_t *)&color_p->full, w, h);
    
    // Inform LVGL flushing complete
    lv_disp_flush_ready(disp);
}

void setup() {
    // Increase serial buffer to accomodate high-frequency JSON updates
    Serial.setRxBufferSize(1024);

    // Set baud rate (should match .NET service)
    Serial.begin(9600);
    
    // Hardware backlight control
    pinMode(5, OUTPUT);
    digitalWrite(5, HIGH); 

    // Initialise display hardware
    gfx->begin();

    // Initialise LVGL core
    lv_init();

    // Setup LVGL display driver
    lv_disp_draw_buf_init(&draw_buf, buf, NULL, screenWidth * 10);
    static lv_disp_drv_t disp_drv;
    lv_disp_drv_init(&disp_drv);
    disp_drv.hor_res = screenWidth;
    disp_drv.ver_res = screenHeight;
    disp_drv.flush_cb = my_disp_flush;
    disp_drv.draw_buf = &draw_buf;
    lv_disp_drv_register(&disp_drv);

    // Build UI
    ui_init();
    ui_initialised = true;
}

void loop() {
    // Maintain LVGL internal clock, tell LVGL 5ms has passed.
    lv_tick_inc(5);

    // Process UI tasks (update UI, animations and transitions)
    lv_timer_handler();

    // Process serial data
    if (ui_initialised) {
        handle_serial_input();
    }

    // Yield small delay to keep system stable
    delay(5);
}