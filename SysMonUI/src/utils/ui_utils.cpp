#include "ui_utils.h"

/**
 * @brief Safely extracts a JsonObject from a JsonDocument.
 * @param document The source JsonDocument (passed by reference to avoid memory copies).
 * @param jsonKey The key string to look for (e.g., "Processor").
 * @return JsonObject The requested object, or a 'Null' JsonObject if the key is missing.
 */
JsonObject try_get_json_object(JsonDocument& document, const char* jsonKey) {
    return document[jsonKey].as<JsonObject>();
}

/**
 * @brief Safely extracts a float value from a JsonObject.
 * @param object The JsonObject to search within.
 * @param jsonKey The key for the specific sensor value (e.g., "UsagePercentage").
 * @return float The numeric value if found and valid; otherwise returns NAN (Not a Number).
 * @note Returning NAN allows the UI controllers to detect missing data and 
 * display placeholders (like "---") instead of showing 0.00.
 */
float try_get_json_float(JsonObject object, const char* jsonKey) {
    if (jsonKey == NULL) { // Prevent crash if key pointer is null
        return NAN;
    }

    if (object.isNull()) { // Prevent crash if object is null
        return NAN;
    }
    
    if (!object[jsonKey].is<float>()) { // Returns NAN is key value is not a float
        return NAN;
    }

    // All checks passed, extract and return value.
    return object[jsonKey].as<float>();
}