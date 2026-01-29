#ifndef UI_UTILS_H
#define UI_UTILS_H

#include <ArduinoJson.h>

/**
 * @brief Safely extracts a JsonObject from a JsonDocument.
 * @param document The source JsonDocument (passed by reference to avoid memory copies).
 * @param jsonKey The key string to look for (e.g., "Processor").
 * @return JsonObject The requested object, or a 'Null' JsonObject if the key is missing.
 */
JsonObject try_get_json_object(JsonDocument& document, const char* jsonKey);

/**
 * @brief Safely extracts a float value from a JsonObject.
 * @param object The JsonObject to search within.
 * @param jsonKey The key for the specific sensor value (e.g., "UsagePercentage").
 * @return float The numeric value if found and valid; otherwise returns NAN (Not a Number).
 * @note Returning NAN allows the UI controllers to detect missing data and 
 * display placeholders (like "---") instead of showing 0.00.
 */
float try_get_json_float(JsonObject object, const char* jsonKey);

#endif // UI_UTILS_H

