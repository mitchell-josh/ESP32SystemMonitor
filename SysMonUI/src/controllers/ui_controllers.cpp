#include "ui_controllers.h"
#include "controller_constants.h"
#include "utils/ui_utils.h"

// External C linkage for SquareLine Studio generated UI files
// This allows C++ to access variables and functions defined in the UI C files
extern "C" {
    #include "ui/ui.h"
    #include "ui/screens/ui_Home.h"
}

// Global state for dynamic chart scaling
static float currentMax = 10.0f;

// Forward declaration of the helper function for updating labels
void update_label(lv_obj_t *obj, const char* format, float label);

/**
 * @brief Main entry point for UI updates.
 * Takes a raw JSON string (from Serial), parses it into a document, 
 * and routes data to the specific hardware component controllers.
 * @param jsonString Raw buffer containing the system monitor data.
 */
void update_ui(const char * jsonString) {
    JsonDocument doc; 
    DeserializationError error = deserializeJson(doc, jsonString);

    // If JSON is malformed, log the error and exit to prevent null pointer access
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.f_str());
        return;
    }

    // Extract sub-objects using keys defined in controller_constants.h
    JsonObject processor = try_get_json_object(doc, Keys::PROCESSOR);
    JsonObject graphics = try_get_json_object(doc, Keys::GRAPHICS);
    JsonObject memory = try_get_json_object(doc, Keys::MEMORY);
    JsonObject network = try_get_json_object(doc, Keys::NETWORK);

    // Distribute data to specific component handlers
    update_processor(processor);
    update_graphics(graphics);
    update_memory(memory);
    update_network(network);
}

/**
 * @brief Controller for Processor (CPU) data.
 * Extracts Usage, Clock Speed, and Temperature to update the relevant LVGL labels.
 * @param processor The JsonObject containing CPU-specific keys.
 */
void update_processor(JsonObject processor) {
    if (processor.isNull()) {
        return;
    }

    float usagePercentage = try_get_json_float(processor, Keys::USAGE_PCT);
    float clockSpeed = try_get_json_float(processor, Keys::CLOCK_SPEED);
    float temperature = try_get_json_float(processor, Keys::TEMP);

    update_label(ui_LblProcessorPct, "%.2f %%", usagePercentage);
    update_label(ui_LblProcessorClock, "%.2f GHz", clockSpeed);
    update_label(ui_LblProcessorTemp, "%.2f °c", temperature);
}

/**
 * @brief Controller for Graphics (GPU) data.
 * Updates GPU Usage, Clock Speed, and Temperature to update the relevant LVGL labels.
 * @param graphics The JsonObject containing GPU-specific keys.
 */
void update_graphics(JsonObject graphics) {
    if (graphics.isNull()) {
        return;
    }

    float usagePercentage = try_get_json_float(graphics, Keys::USAGE_PCT);
    float clockSpeed = try_get_json_float(graphics, Keys::CLOCK_SPEED);
    float temperature = try_get_json_float(graphics, Keys::TEMP);

    update_label(ui_LblGraphicsPct, "%.2f %%", usagePercentage);
    update_label(ui_LblGraphicsClock, "%.0f MHz", clockSpeed);
    update_label(ui_LblGraphicsTemp, "%.2f °c", temperature);
}

/**
 * @brief Controller for System Memory (RAM) data.
 * Calculates and displays current memory utilization.
 * @param memory The JsonObject containing RAM-specific keys.
 */
void update_memory(JsonObject memory) {
    if (memory.isNull()) {
        return;
    }

    float usedMemory = try_get_json_float(memory, Keys::USED_MEM);

    update_label(ui_LblMemoryUsage, "%.2f GB", usedMemory);
}

/**
 * @brief Controller for Network activity.
 * Handles Sent/Received throughput and triggers chart updates.
 * @param network The JsonObject containing network-specific keys.
 */
void update_network(JsonObject network) {
    if (network.isNull()) {
        return;
    }

    float receivedData = try_get_json_float(network, Keys::NET_RECV);
    float sentData = try_get_json_float(network, Keys::NET_SENT);

    // Only update if both values were successfully parsed
    if (!isnan(receivedData) && !isnan(sentData)) {
        char buf[25];

        // Combine Sent/Received into a single multi-line label
        snprintf(buf, sizeof(buf), "%.2f / %.2f\n(MBps)", receivedData, sentData);
        lv_label_set_text(ui_LblNetworkDownUp, buf);    
        update_network_charts(sentData, receivedData);
    }
}

/**
 * @brief Updates the visual Chart widgets for network traffic.
 * Manages the data series and dynamic scaling for the Up/Down charts.
 * @param new_sent Current upload speed in MBps.
 * @param new_received Current download speed in MBps.
 */
void update_network_charts(float new_sent, float new_received) {
    // Dynamic Y-axis scaling logic
    if (new_received > currentMax * 0.9) {
        currentMax = new_received * 1.2f; // 20% headroom
        lv_chart_set_range(ui_ChartNetDown, LV_CHART_AXIS_PRIMARY_Y, 0, (lv_coord_t)currentMax);
    }

    // Push new values into the LVGL chart series
    lv_chart_set_next_value(ui_ChartNetUp, ui_chart_series_net_up, (lv_coord_t)new_sent);
    lv_chart_set_next_value(ui_ChartNetDown, ui_chart_series_net_down, (lv_coord_t)new_received);
}

void update_label(lv_obj_t *obj, const char* format, float label) {
    if (obj == NULL) {
        return;
    }

    if (isnan(label)) {
        // Use placeholder defined in controller_constants.h (e.g., "-")
        lv_label_set_text(obj, Values::EMPTY);
    }
    else {
        char buf[25];
        // Format the float into the buffer (e.g., "55.00 %")
        snprintf(buf, sizeof(buf), format, label);
        lv_label_set_text(obj, buf);
    }
}
