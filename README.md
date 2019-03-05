# QuestionGenerator

This was a little side project created to randomly pick questions out of a predefined database and email them to specific people. I primarily built it to practice using some basic tools such as EFCore and sending emails with MailGun, but I fully intend to use it as well.

You can configure the project by updating the appsettings.sample.json file to the fields you would like to use. The private API key and domain will come from your personal MailGun account and you can specify any sender/recipients you want. You will also need to add the connection string to the project, configure the database (EFCore migration already in the repo), and add questions to the database. Then you should be good to go!
