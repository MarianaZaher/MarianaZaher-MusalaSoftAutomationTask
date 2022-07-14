Feature: Contact Us
	Validate functionality of contact us page

@Email
Scenario: InvalidEmailFormat
	Given email text 
	And other valid data
	When click send button
	Then validation message "The e-mail address entered is invalid" should be visible