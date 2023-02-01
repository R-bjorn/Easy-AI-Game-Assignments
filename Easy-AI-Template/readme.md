# Easy-AI

Unity library to easily learn and prototype artificial intelligence for games.
-

- [Overview](#overview "Overview")
- [Assignment 1](#assignment-1 "Assignment 1")
  - [Tutorial 1](#tutorial-1 "Tutorial 1")
- [Assignment 2](#assignment-2 "Assignment 2")
  - [Tutorial 2](#tutorial-2 "Tutorial 2")
- [Assignment 3](#assignment-3 "Assignment 3")
- [Assignment 4](#assignment-4 "Assignment 4")
- [Project](#project "Project")
- [Assets](#assets "Assets")

# Overview

- Easy-AI consists of templates for four assignments that aim to build the skills necessary to complete the included project. Source code is fully commented with guidance to where sections are to be completed in each assignment. Assignments one and two focus on utilizing the Easy-AI library, whereas assignments three and four focus on developing the Easy-AI library itself. Each assignment is designed to be a building block to prepare for the final project of creating a game of capture the flag.

# Assignment 1

Agents, Sensors, and Actuators
-

- In assignment one, you need to help a vacuum cleaning agent keep the floor clean.
- Grey tiles are normal floor tiles which are clean. Shiny white floor tiles are also clean, but they are more likely to become dirty than normal floor tiles.
- Tiles can become either dirty or very dirty, indicated by light brown and dark brown. Tiles will randomly become dirty over time.
- Your objective is to complete the cleaner agent's mind along with any sensors and actuators it needs to complete its task.
- You should also create a performance measure to determine how well the cleaner is doing.

## Tutorial 1

- A sample scene to demonstrate the use of a mind, sensors, actuators, and a performance measure.
- This scene shows an agent sensing the nearest box to it, moving towards it, collecting it, and repeating until there are no more boxes left.

# Assignment 2

Finite State Machines
-

- Now understanding the basics of how agents can interact with the world from assignment one, it’s now time to create more complex behaviours.
- You need to complete the mind and states for microbes to allow them to hunt and mate with each other.
- Some sensors you may find useful to use have already been provided. As microbes can only eat or mate with others of certain colors, these sensors will help you get valid prey or mates.
- Explore the microbe agent API as it provides easy ways to access properties of the microbe such as its hunger and if it has mated yet.
- Pickups are automatically picked up by microbes close enough to them.
- Microbes can mate with and eat various colors as seen in the table below where an "M" means those microbes can mate and a blank space means those microbes can eat each other:

|        | Red | Orange | Yellow | Green | Blue | Purple | Pink |
|:------:|:---:|:------:|:------:|:-----:|:----:|:------:|:----:|
|  Red   |  M  |   M    |        |       |      |        |  M   |
| Orange |  M  |   M    |   M    |       |      |        |      |
| Yellow |     |   M    |   M    |   M   |      |        |      |
| Green  |     |        |   M    |   M   |  M   |        |      |
|  Blue  |     |        |        |   M   |  M   |   M    |      |
| Purple |     |        |        |       |  M   |   M    |  M   |
|  Pink  |  M  |        |        |       |      |   M    |  M   |

## Tutorial 2

- A sample scene demonstrating the use of finite state machines.
- This scene shows an agent moving between two states. When it has energy, it is moving around randomly. When it is out of energy, it rests in place.

# Assignment 3

Steering Behaviours
-

- Our agents can now interact with the world and make more complex decisions, but the available tools to move them are limiting.
- So far, agents can only seek a position. For instance, as seen with microbes, the library is missing behaviour to make a microbe run away when being hunted.
- Complete the flee, pursue, and evade steering behaviours in the Easy-AI library.
- Then, use these completed behaviours to go back and improve the microbes from assignment two, and see how the addition of a few new steering behaviours gives the scene more life.

# Assignment 4

Navigation
-

- Our agents can now move around, but so far, they've only been in open environments. They need to be able to get around obstacles in more complex environments.
- First, complete corner-graph generation in the Easy-AI library.
- Then, complete A* pathfinding in the Easy-AI library.

# Project

Goal-Oriented Behaviour
-

- Each assignment has introduced a building block to creating intelligent agents.
- However, agents thus far have been working individually. Now, they must compete in a team-based game, and team members must take on different roles such as attackers or defenders that work together to complete the greater goal of capturing the enemy flag while not letting their own get captured.
- Use all skills you have learned to have agents compete in a game of capture the flag.
- Some sensors you may find useful to use have already been provided.
- Explore the soldier agent API as it provides easy ways to access properties of the soldier such as its health, current weapon, and the enemies it has detected.
- The soldier manager automatically handles aiming and shooting of the soldiers for you, all you need to do is decide for soldiers who to aim at and the framework will handle the rest for you.
- Capture, attack, and defend roles have been provided for you which are assigned in the soldier class automatically. You may wish to expand or change how this method works with more roles or change how they are assigned to allow for more complex interactions and teamwork.
- Flags, ammo pickups, and health pickups will automatically be picked up by soldiers close enough to them.
- "Strategic points" have been placed around the map in positions that could be good for offence or defence. Consider building upon this idea.

# Assets

- [Retro Medieval Kit | Kenney](https://www.kenney.nl/assets/retro-medieval-kit "Retro Medieval Kit | Kenney")
- [Blaster Kit | Kenney](https://kenney.nl/assets/blaster-kit "Blaster Kit | Kenney")
- [Sci-Fi Sounds | Kenney](https://www.kenney.nl/assets/sci-fi-sounds "Sci-Fi Sounds | Kenney")
- [War FX | Jean Moreno (JMO)](https://assetstore.unity.com/packages/vfx/particles/war-fx-5669 "War FX | Jean Moreno (JMO)")
- [Free Sound Effects Pack | Olivier Girardot](https://assetstore.unity.com/packages/audio/sound-fx/free-sound-effects-pack-155776 "Free Sound Effects Pack | Olivier Girardot")