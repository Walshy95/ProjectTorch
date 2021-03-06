﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script exists for the sole purpose of holding the game's dialogue lines
/// </summary>
public class TextManager : MonoBehaviour {

    #region Enums
    public enum InteractiveNPCNames
    {
        NONE,
        KingOfMan,
        KingOfDark,
        CaptainOfTheGuard,
        Altar,
        ST1Grave1,
        ST1Grave2,
    }
    #endregion

    #region Private Fields
    // A huge batch of every set of dialogue lines in the game
    private Dictionary<string, string[]> lines;
    // Array of NPCs that can be interacted with
    private Entity[] interactiveNPCs;
#endregion

    #region Properties
    public Dictionary<string, string[]> Lines { get { return lines; } }
    public Entity[] InteractiveNPCs { get { return interactiveNPCs; } }
    #endregion

    #region Unity Defaults
    void Awake () {
        //Grab all entities in the scene
        Entity[] entities = GameObject.FindObjectsOfType(typeof(Entity)) as Entity[];
        //Keep hold of ones that are interactive
        int numInteractiveNPCs = 0;
        List<Entity> entitiesToKeep = new List<Entity>();
        for (int i = 0; i < entities.Length; ++i)
        {
            if (entities[i].npcID != InteractiveNPCNames.NONE) 
            {
                ++numInteractiveNPCs;
                entitiesToKeep.Add(entities[i]);
            }
        }
        interactiveNPCs = new Entity[numInteractiveNPCs];
        for (int i = 0; i < numInteractiveNPCs; ++i)
            interactiveNPCs[i] = entitiesToKeep[i];

        //Instantiate the lines dictionary
        lines = new Dictionary<string, string[]>();
        //Here we will add all possible lines of dialogue in the game to the dictionary

        //The first lines the Altar says
        lines.Add("Altar - Default",
            new string[]
            {
                "Altar:\nThe cold stone calls back to a time buried in death.",
                "Altar:\nThe time of your birth, when fire devoured fire."
            });

        //The basic lines the King will say to you with no other triggers
        lines.Add("King of Man - Default",
            new string[]
            {
                "King of Man:\nGood traveler, I know not of your origin, be it the Serpent’s Earth or Dragon’s sky, but I beg of you to aid us in our time of need.",
                "King of Man:\nAid us as we repel the invaders from the shadows so that we may retake our place in this world!",
                "King of Man:\nTheir King’s fortress lies to the west.",
                "King of Man:\nLead the charge as I fulfill the last wish of the Serpents."
            });

        //The lines the King of the Dark says when he's at his last 15% of health and can't move
        lines.Add("King of the Dark - Downed",
            new string[]
            {
                "King of the Dark:\nGive her back… You monsters.",
                "King of the Dark:\nMy daughter is more than a means to your ends.",
                "King of the Dark:\nShe is more than your kindling.",
                "King of the Dark:\nPlease... Do not let your King murder my daughter..."
            });

        //The Captain of the Guard's text when he meets the player in the village after they've protected it
        lines.Add("Captain of the Gaurd - Village",
            new string[]
            {
                "Captain of the Gaurd:\nWell met stranger.",
                "Captain of the Gaurd:\nWe’ve heard word of a foreign warrior defending this village against the oncoming hoards.",
                "Captain of the Gaurd:\nGather your strength friend, for today we retake our birthright.",
                "Captain of the Gaurd:\nWe will seize the opportunity you’ve opened here and charge into the heart of the enemy!"
            });

        //The Captain of the Guard's text when he reaches the Sentinels
        lines.Add("Captain of the Gaurd - Sentinels",
            new string[]
            {
                "Captain of the Gaurd:\nThe strength of man shall not falter here.",
                "Captain of the Gaurd:\nWhile we can force the way open, their reinforcements shall not sit idly.",
                "Captain of the Gaurd:\nWhen the gate opens, you, the strongest of us all, should bring a close to this war.",
                "Captain of the Gaurd:\nWe will hold the line here as you battle their King.",
                "Captain of the Gaurd:\nHerald us all to the Serpents’ Promised Flame!"
            });
        //TEMP - The dude escorting the princess
        lines.Add("Princess Escort - Temp",
            new string[]
            {
                "Super Mario:\nNice of the princess to invite us over for a picnic, eh Luigi?",
                "Mama Luigi:\nI hope she made lotsa spaghetti!"
            });
        //TEMP - The princess when she's saved
        lines.Add("Princess - Saved",
            new string[]
            {
                "Princess:\nOh HELL yeah, you saved me man!",
                "Princess:\nI'm gonna go chill with my dad now"
            });
        //Battlefield Brazier
        lines.Add("Brazier - Battlefield",
            new string[]
            {
                "Brazier:\nThe path ahead. Illuminated by dragon’s fire,",
                "Brazier:\ncradled in a serpent’s pyre. How rare."
            });
        //HumanTerritoryStage1 Brazier
        lines.Add("Brazier - HumanTerritoryStage1",
            new string[]
            {
                "Brazier:\nLeft behind to wallow in the waters, the serpents gazed at the sky.",
                "Brazier:\nLooking on as the Dragons forged their domain: The Sky."
            });
        //HumanTerritoryStage2 Brazier
        lines.Add("Brazier - HumanTerritoryStage2",
            new string[]
            {
                "Brazier:\nEver watching, the World Serpent lay in wait.",
                "Brazier:\nA day came where a dragon drew near to the world below.",
                "Brazier:\nAnd he was devoured by the World Serpent.",
                "Brazier:\nThe first murder."
            });
        //ShadowTerritoryStage1 Brazier
        lines.Add("Brazier - ShadowTerritoryStage1",
            new string[]
            {
                "Brazier:\nIn the beginning, where the depths housed the world, there were only two kinds to speak of.",
                "Brazier:\nThose with wings; those without. Dragons and serpents."
            });
        //WarZone Brazier
        lines.Add("Brazier - WarZone",
            new string[]
            {
                "Brazier:\nBorn from nothing, the soul of a dragon took the shape of man.",
                "Brazier:\nIn disgust, it was cast out in a Dragon's Exile.",
                "Brazier:\nAn impossible creature, tasked with an impossibility."
            });
        //ShadowTerritoryStage1 Grave1
        lines.Add("ST1Grave1 - Default",
            new string[]
            {
                "Makeshift Grave:\nThe cry of a princess, drowned by the will of man.",
                "Makeshift Grave:\nTheir King’s decree and prophet’s suggestion, so is the power in words.",
                "Makeshift Grave:\nWords which weigh heavier than my life."
            });
        //ShadowTerritoryStage1 Grave2
        lines.Add("ST1Grave2 - Default",
            new string[]
            {
                "Makeshift Grave:\nWhen the plague spread over us, it took with it our pride.",
                "Makeshift Grave:\nThe pride of man was broken.",
                "Makeshift Grave:\nPerhaps, it is the great serpents calling for us to join them in death?"
            });
    }
#endregion
}
