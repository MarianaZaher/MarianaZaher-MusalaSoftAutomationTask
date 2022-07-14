Feature: Apply for Career
	Applying for Position

@career
Scenario: Apply For Automation QA Engineer Position
	Given position is Automation QA Engineer
	And location is Sofia
	And some data is not valid
	When click apply button
	Then validation message should appear

	Scenario: Print in the console the list with available positions by city 
	Given location is Sofia
	When click select location
	Then list of available positions filtered