Instructions TO RUN:
1. Apply the relevant connection string in appsettings.json.

2. Run the application, and pass the command from console.

3. Available Commands:.
		ADD DD/MM HH:mm - Add an appointment.
		DELETE DD/MM HH:mm - Delete an appointment.
		FIND DD/MM - Find free time slots.
		KEEP HH:mm - Keep a time slot for any day.
		EXIT - Exit the application.
		Enter command:.
		
4. For any changes to model, run the below commands as it's EFCore Code first    
   approach,
   
		Add-Migration InitialCreate
		Update-Database


AREAS covered:
1. Most of the requirements part except the remaining logic in KEEP command

AREAS needs improvement:
1. KEEP logic is not clear, open to discuss
2. Needs to work on FixedSlots and scripts.
3. Configurable parameter or keys in key vault.
4. Create a Relational database with more entities and include normalisation    
   principles.
5. Use of Cloud platform and services for hosting
6. Use of front-end technology for UI experience
7. Logging and exception handling
