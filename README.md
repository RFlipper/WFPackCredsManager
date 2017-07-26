# Workflow Actions Pack Credentials Manager
A small console utility that helps to manage credentials for Plumsail Workflow Actions Pack

## Intro
[Workflow Actions Pack](https://plumsail.com/workflow-actions-pack/) uses user credentials to execute required actions in some cases it can be a big advantage and in some, it could make some issues. 
For example, Azure Active Directory has some default password expiration policy that requires users to change passwords each 90 day. 
We recommend creating a separate service account for Action Pack and turn off password expiration policy for the account.

**Set a password to never expire**
1.	Connect to Windows PowerShell using your company administrator credentials.
2.	Execute one of the following commands:
•	To set the password of one user to never expire, run the following cmdlet by using the user principal name (UPN) or the user ID of the user: `Set-MsolUser -UserPrincipalName <user ID> -PasswordNeverExpires $true`
•	To set the passwords of all the users in an organization to never expire, run the following cmdlet: `Get-MSOLUser | Set-MsolUser -PasswordNeverExpires $true`

## The tool 
However, let's imagine that the password was changed and you found that workflows don't work and you need to manually find all sites where Workflow Actions Pack was activated and to log in to password management page using a new account. Definitely, you will not very happy about it.

Some of our users have already had this situation and invented some workarounds, you can check out the following [forum thread](https://plumsail.com/forum/viewtopic.php?f=22&t=482&p=1657). 

Actually, the task is very trivial and in below I want to describe my solution. 
I wrote a small console utility that accepts *URL, Login, Password* and connects to SharePoint. After it, it remembers Workflow Actions Pack credentials from root site and shows all accounts from child's sites. 

If you run this tool with `-f` flag it will change Workflow Actions Pack default login and password credentials for all child's sites.

`WFPackCredsManager.exe -u https://yourtenant.sharepoint.com/sites/test -l admin@yourtenant.onmicrosoft.com -p P@ssword1 -f`

### Examples
![Help](https://raw.githubusercontent.com/RFlipper/WFPackCredsManager/master/imgs/console-help.png "How to use")
![Sample](https://raw.githubusercontent.com/RFlipper/WFPackCredsManager/master/imgs/console-inaction.png "Sample")


