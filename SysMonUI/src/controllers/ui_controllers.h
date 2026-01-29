#ifndef UI_CONTROLLERS_H
#define UI_CONTROLLERS_H

#include <ArduinoJson.h>
#include <lvgl.h>
#include "ui/ui.h"

/**
 * @brief Main entry point for UI updates.
 * Takes a raw JSON string (from Serial), parses it into a document, 
 * and routes data to the specific hardware component controllers.
 * @param jsonString Raw buffer containing the system monitor data.
 */
void update_ui(const char * jsonString);

/**
 * @brief Controller for Processor (CPU) data.
 * Extracts Usage, Clock Speed, and Temperature to update the relevant LVGL labels.
 * @param processor The JsonObject containing CPU-specific keys.
 */
void update_processor(JsonObject processor);

/**
 * @brief Controller for Graphics (GPU) data.
 * Updates GPU Usage, Clock Speed, and Temperature to update the relevant LVGL labels.
 * @param graphics The JsonObject containing GPU-specific keys.
 */
void update_graphics(JsonObject graphics);

/**
 * @brief Controller for System Memory (RAM) data.
 * Calculates and displays current memory utilization.
 * @param memory The JsonObject containing RAM-specific keys.
 */
void update_memory(JsonObject memory);

/**
 * @brief Controller for Network activity.
 * Handles Sent/Received throughput and triggers chart updates.
 * @param network The JsonObject containing network-specific keys.
 */
void update_network(JsonObject network);

/**
 * @brief Updates the visual Chart widgets for network traffic.
 * Manages the data series and dynamic scaling for the Up/Down charts.
 * @param new_sent Current upload speed in MBps.
 * @param new_received Current download speed in MBps.
 */
void update_network_charts(float new_sent, float new_received);

#endif // UI_CONTROLLERS_H