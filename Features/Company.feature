Feature: Company
	validate company tab and page url

@company
Scenario: ValidateCompanyTab
	When Navigate to http://www.musala.com/
    And Click ‘Company’ tap 
	Then  the correct URL (http://www.musala.com/company/) loads
	Then that there is ‘Leadership’ section
	When the Facebook link from the footer
	Then the correct URL (https://www.facebook.com/MusalaSoft?fref=ts) loads 
