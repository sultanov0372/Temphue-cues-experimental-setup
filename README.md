# Temphue cues experimental setup
This repository is the experimental setup made in the frame of my Bachelor thesis, to test how thermal feedback can be provided in VR only using visual cues


## Setup & Instructions
 
 Follow the instructions below to set up and run the experiment.

---

## Prerequisites

* **Unity version:** `2022.3.33f1` (required for compatibility)
  
---

## Project Structure

The key playable scenes can be found in:

```
Scenes/Important/
```

### Object Interaction Scenes

* `Question_train2` – Training scene with no feedback
* `Question_Combined` – Object interaction with combined visualisation
* `Question_Particles` – Object interaction with out-of-hand visualisation
* `Question_HandColor` – Object interaction with in-hand visualisation

### Social Interaction Scenes

* `HS_Combined` – Social interaction with combined visualisation
* `HS_Particles` – Social interaction with out-of-hand visualisation
* `HS_HandColor` – Social interaction with in-hand visualisation

---

## Controls

### General Controls (all scenes)

* **O** – Reload the current scene

### Social Interaction Scenes

* **K** – Start avatar animation
* **P** – After the interaction finishes, load the corresponding questionnaire scene:

  * `HS_comb_quest` (for `HS_Combined`)
  * `HS_Part_quest` (for `HS_Particles`)
  * `HS_HC_quest` (for `HS_HandColor`)
  
* **I** – Load the previous scene

---

## Questionnaire & Data Storage

* The **VRQuestionnaireToolkit** GameObject contains the **Participant ID** field. Update this before running the experiment.
* After completing a questionnaire:

  * Data is saved as a CSV in:

    ```
    Questionnaires/Data/Answers/
    ```
  * Answers are also appended to a **common for each interaction CSV file** containing all participants’ responses.
  The name would be for example `questionnaireID_ISOcan_ALL_answers` (ISOcup/ISOhsPart etc.)

---

## Notes

* The button press works only if you are colliding with the object or the avatar with the other hand at the moment. It will write down the time before the press in the console and the questionnaire will appear.
---

## Contact

For questions or issues, please contact me via this email: `artem.temon1@gmail.com`
