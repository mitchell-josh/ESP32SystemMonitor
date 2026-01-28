#include <Arduino_GFX_Library.h>
#include <ArduinoJson.h>
#include <lvgl.h>

// This is the magic bridge to your SquareLine files
extern "C" {
    #include "ui/ui.h"
    #include "ui/screens/ui_Home.h"
}

/* Hardware & Buffer Setup (Keep your working code here) */
Arduino_DataBus *bus = new Arduino_ESP32SPI(41, 42, 40, 45, -1);
Arduino_GFX *gfx = new Arduino_ST7789(bus, 39, 1, true);

static const uint32_t screenWidth  = 320;
static const uint32_t screenHeight = 240;
static lv_disp_draw_buf_t draw_buf;
static lv_color_t buf[screenWidth * 10];

/* Network monitor variables */
float sentData = 0;
float receivedData = 0;
float currentMax = 10;

/* Function prototypes */
void update_processor(JsonDocument doc);
void update_graphics(JsonDocument doc);
void update_memory(JsonDocument doc);
void update_network(JsonDocument doc);
void update_network_charts(float new_sent, float new_received);
void update_ui(const char * jsonString);
void handle_serial_input();
void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p);

void update_processor(JsonDocument doc) {
    JsonObject processor = doc["Processor"];
    if (!processor.isNull()) {
        if (processor.containsKey("UsagePercentage")) {
            float usagePercentage = processor["UsagePercentage"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f %%", usagePercentage);
            lv_label_set_text(ui_LblProcessorPct, buf);
        }
        if (processor.containsKey("ClockSpeed")) {
            float clockSpeed = processor["ClockSpeed"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f GHz", clockSpeed);
            lv_label_set_text(ui_LblProcessorClock, buf);
        }
        if (processor.containsKey("Temperature")) {
            float temperature = processor["Temperature"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f °c", temperature);
            lv_label_set_text(ui_LblProcessorTemp, buf);
        }
    }
}

void update_graphics(JsonDocument doc) {
    JsonObject graphics = doc["Graphics"];
     if (!graphics.isNull()) {
        if (graphics.containsKey("UsagePercentage")) {
            float usagePercentage = graphics["UsagePercentage"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f %%", usagePercentage);
            lv_label_set_text(ui_LblGraphicsPct, buf);
        }
        if (graphics.containsKey("ClockSpeed")) {
            float clockSpeed = graphics["ClockSpeed"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.0f MHz", clockSpeed);
            lv_label_set_text(ui_LblGraphicsClock, buf);
        }
        if (graphics.containsKey("Temperature")) {
            float temperature = graphics["Temperature"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f °c", temperature);
            lv_label_set_text(ui_LblGraphicsTemp, buf);
        }
    }
}

void update_memory(JsonDocument doc) {
    JsonObject memory = doc["Memory"];
    if (!memory.isNull()) {
        if (memory.containsKey("UsedMemory")) {
            float usedMemory = memory["UsedMemory"];
            char buf[16];
            snprintf(buf, sizeof(buf), "%.2f GB", usedMemory);
            lv_label_set_text(ui_LblMemoryUsage, buf);
        }
    }
}

void update_network(JsonDocument doc) {
    JsonObject network = doc["Network"];
    if (!network.isNull()) {
        if (network.containsKey("Sent") && network.containsKey("Received")) {
            sentData = network["Sent"];
            receivedData = network["Received"];

            char buf[25];
            snprintf(buf, sizeof(buf), "%.2f / %.2f\n(MBps)", receivedData, sentData);
            lv_label_set_text(ui_LblNetworkDownUp, buf);

            update_network_charts(sentData, receivedData);
        }
    }
}

void update_network_charts(float new_sent, float new_received) {
    if (new_received > currentMax * 0.9) {
        currentMax = new_received * 1.2f;
        lv_chart_set_range(ui_ChartNetDown, LV_CHART_AXIS_PRIMARY_Y, 0, (lv_coord_t)currentMax);
    }

    lv_chart_set_next_value(ui_ChartNetUp, ui_chart_series_net_up, (lv_coord_t)new_sent);
    lv_chart_set_next_value(ui_ChartNetDown, ui_chart_series_net_down, (lv_coord_t)new_received);
}

void update_ui(const char * jsonString) {
    Serial.println(jsonString);
    JsonDocument doc; 
    DeserializationError error = deserializeJson(doc, jsonString);
    
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.f_str());
        return;
    }
    
    update_processor(doc);
    update_graphics(doc);
    update_memory(doc);
    update_network(doc);
}

void handle_serial_input() {
    static String incomingBuffer = "";
    static int braceCount = 0;

    while (Serial.available()) {
        char c = (char)Serial.read();
        incomingBuffer += c;

        if (c == '{') braceCount++;
        if (c == '}') braceCount--;

        // If we have at least one brace and they are now balanced
        if (braceCount == 0 && incomingBuffer.indexOf('{') != -1) {
            incomingBuffer.trim();
            update_ui(incomingBuffer.c_str());
            
            // Reset for next full object
            incomingBuffer = "";
        }
        
        // Reset if buffer exceeded
        if (incomingBuffer.length() > 1024) {
            incomingBuffer = "";
            braceCount = 0;
        }
    }
}

void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p) {
    uint32_t w = (area->x2 - area->x1 + 1);
    uint32_t h = (area->y2 - area->y1 + 1);
    gfx->draw16bitBeRGBBitmap(area->x1, area->y1, (uint16_t *)&color_p->full, w, h);
    lv_disp_flush_ready(disp);
}

void setup() {
    Serial.setRxBufferSize(1024);
    Serial.begin(9600);
    
    pinMode(5, OUTPUT);
    digitalWrite(5, HIGH); 
    gfx->begin();

    lv_init();
    lv_disp_draw_buf_init(&draw_buf, buf, NULL, screenWidth * 10);
    static lv_disp_drv_t disp_drv;
    lv_disp_drv_init(&disp_drv);
    disp_drv.hor_res = screenWidth;
    disp_drv.ver_res = screenHeight;
    disp_drv.flush_cb = my_disp_flush;
    disp_drv.draw_buf = &draw_buf;
    lv_disp_drv_register(&disp_drv);
    ui_init();
}

void loop() {
    lv_tick_inc(5);    // Tell LVGL 5ms has passed
    lv_timer_handler(); // Updates the UI, animations, and transitions
    handle_serial_input();
    delay(5);
}