# Final product demo

[Demo](https://www.dropbox.com/scl/fi/kjlx196r7qq0j17l0rreb/StoryWatch_demo.mp4?rlkey=wrer2ou1y7tjtvzgtn2wihqtv&dl=0)

Note: Current backend server is inactive so login/registration and fetching data are not enabled. Dockerfile will be created to allow testing on local machine.

# Domain Description

Content management system about media (movies, games, and books). The idea arose from the fact that most younger generations follow various content from popular culture and entertainment, i.e., books, movies, games, series, anime, etc. Individuals who consume a lot of content may have their own lists of what else they would like to watch/play/read, as well as what they have already consumed to remind themselves of what they liked or what their favorites are, so they can revisit them in the future or watch/play/etc. again when they're in the mood for it. For the user to be able to manage such content in a practical way, a desktop application would serve, consisting of three groups (games, books, movies + possibly custom groups) within which the user can enter movie, book, or game titles via a short form by categories (TODO, watched/played/read, favorites + custom defined categories), search, update, and delete entered content. The application would use information from IMDb and other portals with open APIs for detailed information when displaying a title, and it would have a recommendation system (Recommend) which, based on various factors, would provide content recommendations for today upon request from the user, and it would have an option to generate reports on something (e.g., which genre the user prefers the most based on entered titles). Intended for cinephiles, gamers, and anyone else who thinks such an application would be useful to keep track of all their consumed content and what they plan to consume.

# Project Specification

We will use a three-layer architecture, the highest layer being: presentation, the next layer: business, and the lowest: data layer.

Code	| Name	| Brief Description	| Team Member Responsible
------ | ----- | ----------- | -------------------
F01	| Login and Registration	| Users can register in the application so that multiple users using the same computer can access their own content. The user logs into their own account with the data they received during registration.	| [HLuksic](https://github.com/HLuksic)
F02	| CRUD Functionality for Games | Data entry via form, updating, searching, and deleting data for game titles. In addition to the data entered by the user, data retrieved from web services with an open API (e.g., IGDB) are displayed for game titles. | [HLuksic](https://github.com/HLuksic)
F03	| Recommendation System for Games	| Users can click on an option to be recommended something to play, by filling out a short questionnaire with a few questions with icons as answers (e.g., we have a few offered emojis for genres - chill/scary/action...), whether they would like to play something again or something new), then based on that, but also on other factors, a list of recommendations is provided to the user. The recommendation algorithm would, for example, give a certain number of points if a certain content is on the favorite list, if it has a higher rating on IGDB, etc.	| [HLuksic](https://github.com/HLuksic)
F04	| Report Generation for Games	| The user will have the option to generate a report with visual elements (graphs) on some aspect related to games, e.g., distribution of genres the user prefers based on entered titles, distribution of companies that have released the user's favorite games, etc.	| [HLuksic](https://github.com/HLuksic)
F05	| CRUD Functionality for Movies	| Data entry via form, updating, searching, and deleting data for movie titles. In addition to the data entered by the user, data retrieved from web services with an open API (e.g., IMDb) are displayed for movie titles.	| [nmidzic20](https://github.com/nmidzic20)
F06	| Movie Trailer within the Application	| For the selected movie title, there is an option to watch the trailer, which will fetch the trailer from a web service and display it to the user within an embedded video player in the application.	| [nmidzic20](https://github.com/nmidzic20)
F07	| Recommendation System for Movies	| Users can click on an option to be recommended something to watch, by filling out a short questionnaire with a few questions with icons as answers (e.g., we have a few offered emojis for genres - chill/scary/action...), whether they would like to watch something again or something new), then based on that, but also on other factors, a list of recommendations is provided to the user. The recommendation algorithm would, for example, give a certain number of points if a certain content is on the favorite list, if it has a higher rating on IMDb, etc.	| [nmidzic20](https://github.com/nmidzic20)
F08	| Report Generation for Movies	| The user will have the option to generate a report with visual elements (graphs) on some aspect related to movies, e.g., distribution of genres the user prefers based on entered titles, distribution of directors and actors related to the user's favorite movies, etc.	| [nmidzic20](https://github.com/nmidzic20)
F09	| CRUD Functionality for Books	| Data entry via form, updating, searching, and deleting data for book titles. In addition to the data entered by the user, data retrieved from web services with an open API (e.g., Google Books) are displayed for book titles.	| [davidkajzogaj](https://github.com/davidkajzogaj)
F10	| E-book Preview | Within the Application	For the selected book title, if it is available in electronic format on a web service and has a preview, there is an option to read the preview, which will fetch the preview from a web service (e.g., Google Books) and display it to the user within the application. | [davidkajzogaj](https://github.com/davidkajzogaj)
F11	| Recommendation System for Books	| Users can click on an option to be recommended something to read, by filling out a short questionnaire with a few questions with icons as answers (e.g., we have a few offered emojis for genres - chill/scary/action...), whether they would like to read something again or something new), then based on that, but also on other factors, a list of recommendations is provided to the user. The recommendation algorithm would, for example, give a certain number of points if a certain content is on the favorite list, if it has a higher rating on the web service, etc.	| [davidkajzogaj](https://github.com/davidkajzogaj)
F12	| Report Generation for Books	| The user will have the option to generate a report with visual elements (graphs) on some aspect related to books, e.g., distribution of genres the user prefers based on entered titles, distribution of authors/publishers related to the user's favorite books, etc.	| [davidkajzogaj](https://github.com/davidkajzogaj)

# Non-functional requirements:

- Simple navigation (transition between screens)
- Intuitiveness of the graphical user interface for the user (UX/UI, understandable icons, all options available where expected)
- Fetching data from the web and displaying it in an acceptable time (no delays in the application)
- Validation of the data entered by the user (e.g., if a year is expected for input, the application will not crash if something else is entered)
- F1 document that textually or visually provides a hint related to the part where the user is in the application and where they need help

# Technologies and Equipment

- GitHub Wiki pages for writing technical and project documentation
- Git and GitHub for software versioning
- GitHub Projects for project management i.e., task division and progress tracking/phases of the project (Kanban, relevant part of the Wiki documentation can be referenced in tasks)
- .NET Framework/Core as the development framework
- Type of project: WinForm/WPF/UWP
- Visual Studio 2022 (Community) as the IDE
- Microsoft SQL Server and SQL Server Management Studio for table creation
- Third-party libraries from NuGet Package Manager
