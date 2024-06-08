# Process Repair Data Traceability

## Project Overview

**Project Title**: Process Repair Data Traceability  
**Client Name**: Honda Motorcycle & Scooters India Pvt Ltd  
**Project Used In**: Repair Table of Engine Assembly Line

## Project Purpose

This project is used in the engine assembly department to ensure data traceability during the engine repair process. The application tracks the repair process of engines, particularly focusing on leak testing and defect management.

### Key Processes

1. **Engine Leak Testing**:
    - Engines are routinely leak-tested after certain assembly steps.
    - Engines with detected leaks are moved to the repair table.

2. **Repair Table Workflow**:
    1. **Barcode Scanning**: The barcode on the engine is scanned.
    2. **Initial Leak Testing**: The initial (before) leak test value is saved.
    3. **Repair Work**: The necessary repairs are performed on the engine.
    4. **Final Leak Testing**: The final (after) leak test value is saved.
    5. **Defect Details**:
        - The operator selects the department related to the defect.
        - The defect found during engine repair is selected.
        - Based on the defect, the action dropdown is filled, and the operator selects the appropriate action.
    6. **Remarks**: The operator enters any additional remarks.

## Project Components

### Master Forms

1. **Department Master**: Manages departments involved in the repair process.
2. **Defect Master**: Manages the defects that can be detected in the engines.
3. **Action Master**: Manages actions that can be taken to rectify defects.
4. **User Master**: Manages users of the application.

### Transaction Forms

1. **Defect Entry**: Form for entering defect details and related actions.
2. **Reports**: Generates reports based on defect entries and other data.

## Prerequisites

- Visual Studio 2019 or later
- SQL Server or SQL Server Express
- .NET Framework 4.7.2 or later

## Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/yourusername/ProcessRepairDataTraceability.git
   cd ProcessRepairDataTraceability
