#ifndef CONTROLLER_CONSTANTS_H
#define CONTROLLER_CONSTANTS_H

/**
 * @namespace Keys
 * @brief Contains the JSON keys expected from the C# System Monitor Service.
 */
namespace Keys {
    const char* const PROCESSOR = "Processor";
    const char* const GRAPHICS = "Graphics";
    const char* const MEMORY = "Memory";
    const char* const NETWORK = "Network";

    const char* const USAGE_PCT = "UsagePercentage";
    const char* const CLOCK_SPEED = "ClockSpeed";
    const char* const TEMP = "Temperature";
    const char* const USED_MEM = "UsedMemory";
    const char* const NET_SENT = "Sent";
    const char* const NET_RECV = "Received";
}

/**
 * @namespace Values
 * @brief Default display values for the UI labels.
 */
namespace Values {
    const char* const EMPTY = "-";
}

#endif // CONTROLLER_CONSTANTS_H