## Hackathon entry:
This project is/was an entry for the "Mixed Reality Challange: StereoKit" [devpost](https://mixed-reality-stereokit.devpost.com/)

## Inspiration
The Inspiration for this project came to me in the form of a wish. A colleague from my wife (both work in a chemistry lab) knows that I work with "these strange Headsets". So he let me know he would love to see and explore molecules in 3d. I thought this would be a nice use-case and this hackathon finally "gave me this last push" to start this project.
 
## What it does
The App is a Mixed Reality User Interface for some functionally of the API from [PubChem](https://pubchem.ncbi.nlm.nih.gov/) ([API-Doku](https://pubchemdocs.ncbi.nlm.nih.gov/pug-rest)).
The App allows to search for molecules and displays the calculated 3d structure of the found molecule. Also, it shows a provided description, the full name and lets the user search for structurally similar molecules. 
Because the user can move all the molecules around it is easy to "overlay" molecules and look for differences.

## How I built it
The starting point is a simple dialog with a search field and two example molecules. Either selecting a sample or searching for text, both will send a request to the PubChem Rest API and get back the 3d description of the molecule (in JSON). This description contains all the information needed to build a 3d model containing the different atoms and all the different bindings between it.
Different REST endpoints are used to find similar molecules, descriptions, and other relevant information about the molecules.

## Challenges I ran into
To not disrupt the main application loop all the REST-Calls are done in separated threads this leads to some strange behavior because the model (and materials) were initially created in separate threads. Strangely the different platforms (Quest, HL2 and Standalone also behave all differently). 
Also when using custom text styles (colors) all the text disappeared after some random time (depending on how much text was drawn in each frame). 

## Accomplishments that I'm proud of
The app is working and (nearly) behaving the same on three different platforms. And it feels great to explore the molecules and to search for more complex molecules every time I use it.

## What I learned
It is really easy and fast to get first results with sterokit! But it's still a little quirky when trying to polish an application. 

## What's next for molecule-browser XR
Sadly right now it's again hard to meet people so I was not able to show the application to the person who initially had the idea and gave me the inspiration. I would love to show the application and get feedback from people who are "experts with molecules" and potentially see what the application needs to give them a real benefit for their daily life. Especially in combination with a HoloLens 2 or an Oculus Quest it would be easy to have the device just lay ate their workplace and start-up Molecula when ever needed ;)

## Video

[running on HoloLens 2](https://youtu.be/dxt2jN2iWis)

[running on Oculus Quest](https://youtu.be/tkRDj_Cmoyc)