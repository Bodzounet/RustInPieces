using UnityEngine;
using System.Collections.Generic;

public static class OpCode_SkillNodes
{
    public static Dictionary<int, NodeData> opCodeToNode = new Dictionary<int, NodeData>();

    static OpCode_SkillNodes()
    {
        // Map
        opCodeToNode[1000] = new NodeData("Banish", 3, new int[] { 500 }, "Si la prochaine attaque de chaque héros allié touche un Héros adverse, ce Héros est renvoyé à sa base");
        opCodeToNode[1001] = new NodeData("TowerSpawn", 3, new int[] { 500 }, "La tour la plus proche devient le point d'apparition des minions à la place du QG tant qu'elle n'est pas détruite");
        opCodeToNode[1002] = new NodeData("LaneImprovment", 3, new int[] { 500 });

        opCodeToNode[1003] = new NodeData("Portal", 2, new int[] { 300 });
        opCodeToNode[1004] = new NodeData("GlassWall", 2, new int[] { 300 });
        opCodeToNode[1005] = new NodeData("CheaperObjectives", 2, new int[] { 300 });
        opCodeToNode[1007] = new NodeData("RingOfFire", 2, new int[] { 300 }, "Un anneau de feu apparait autour de la position ciblé, infligeant d'importants dégâts à ceux qui le traversent");
        opCodeToNode[1012] = new NodeData("BackFire", 2, new int[] { 300 }, "Les tours renvoient une partie des dégâts subit. Plus elle subissent dans une court laps de temps, plus le renvoit est important");

        opCodeToNode[1006] = new NodeData("FalseBonusBox", 1, new int[] { 150, 250 }, "Rajoute la possibilité de créer des bonus boxes pièges sur le Box ennemis");
        opCodeToNode[1008] = new NodeData("TowerShield", 1, new int[] { 150, 250 }, "place un bouclier sur une tour pour la protéger, la puissance du bouclier augmente avec le niveau");

        opCodeToNode[1009] = new NodeData("UpgradedBonusBox", 0, new int[] { 100, 150, 250 }, "Rajoute des options à la bonusBox : \nLvl1 : Mana + Vie\nLvl2 : Vitesse\nLvl3 : Mana + Vie + Vitesse");
        opCodeToNode[1010] = new NodeData("HealingTower", 0, new int[] { 100, 150, 250 }, "La tour soigne les alliés à proximité : \nLvl1 : soigne les minions\nLvl2 : soigne aussi les Héros\nLvl3 : soigne également la tour");
        opCodeToNode[1011] = new NodeData("RootTrap", 0, new int[] { 100, 150, 250 }, "Pose un piège handicapant sur un Héros ennemi : \nLvl1 : ralentit sa vitesse\nLvl2 : immobilise le Héros\nLvl3 : assome le Héros");
        

        // Heroes
        opCodeToNode[1100] = new NodeData("GoldenShield", 3, new int[] { 500 });
        opCodeToNode[1101] = new NodeData("BiggerBags", 3, new int[] { 500 }, "Rajoute un slot d'objet pour chaque Héros allié");
        opCodeToNode[1102] = new NodeData("SpiritualLink", 3, new int[] { 500 }, "Tout les Héros alliés se partagent équitablement les dégâts reçus jusqu'à la mort de l'un d'entre eux");

        opCodeToNode[1104] = new NodeData("DreadfulAura", 2, new int[] { 300 }, "Une aura à placer sur un Héros allié, qui réduit les stats des ennemis dans son rayon d'action");
        opCodeToNode[1110] = new NodeData("ProtectiveAura", 2, new int[] { 300 }, "Une aura à placer sur un Héros allié, qui améliore les stats des alliés dans son rayon d'action");

        opCodeToNode[1105] = new NodeData("GreedyHeroes", 1, new int[] { 150, 250 }, "Lvl1 : Les Héros gagnent un peu plus d'or en tuant des sbires ennemis\nLvl2: Les Héros gagnent plus d'or de façon passive");
        opCodeToNode[1106] = new NodeData("ReloadFaster", 1, new int[] { 150, 250 });
        opCodeToNode[1107] = new NodeData("RebirthSwiftness", 1, new int[] { 150, 250 }, "Lors de leur réapparition, les Héros alliés bénéficient d'un boost de vitesse");
        opCodeToNode[1108] = new NodeData("Trader", 1, new int[] { 150, 250 });

        opCodeToNode[1103] = new NodeData("DivineShield", 0, new int[] { 150, 250, 250 }, "Pose un bouclier sur un Héros allié, qui absorbe une certain quantité de dégât. la quantité augmente avec le niveau");
        opCodeToNode[1109] = new NodeData("Steroids", 0, new int[] { 100, 150, 250 }, "Boost les stats d'un héros allié, la puissance du boost augmente avec le niveau");

        // Minions
        opCodeToNode[1200] = new NodeData("Badass", 3, new int[] { 500 });
        opCodeToNode[1201] = new NodeData("Duplicate", 3, new int[] { 500 }, "Chaque minion allié présent dans l'arène se dédouble.");
        opCodeToNode[1202] = new NodeData("HurryUp", 3, new int[] { 500 }, "Les minions apparaissent 50% plus vite pendant 120 secondes.");
        opCodeToNode[1203] = new NodeData("OnRampage", 3, new int[] { 500 }, "Les minions gagnent des bonus à toutes leurs stats.");
        opCodeToNode[1204] = new NodeData("ThisOneMustDie", 2, new int[] { 300 }, "Le héros ciblé subit 2 fois plus de dégâts par les minions.");
        opCodeToNode[1205] = new NodeData("GreedyMinions", 2, new int[] { 300 });
        opCodeToNode[1206] = new NodeData("Herault", 2, new int[] { 300 }, "Rajoute une carte Hérault au Minion Manager.\nLe Hérault booste considérablement les minions et héros alliés");
        opCodeToNode[1207] = new NodeData("Slots", 1, new int[] { 150, 250 }, "Rajoute des slots pour minions dans le Minion Manager\nLvl1 : +2 slots\nLvl2 : +2 slots.");
        opCodeToNode[1208] = new NodeData("Tank", 1, new int[] { 150, 250 }, "Rajoute des cartes Tanks au Minion Manager. Les tanks encaissent très bien les dégâts.\nLvl1 : cartes++\nLvl2 : cartes++, nouveau sort de tanking.");
        opCodeToNode[1209] = new NodeData("Healer", 1, new int[] { 150, 250 }, "Rajoute des cartes Healers au Minion Manager. Les Healers soigent les minions alliés mais n'attaquent pas.\nLvl1 : cartes++\nLvl2 : cartes++, régénères les alliés lors de sa mort.");
        opCodeToNode[1210] = new NodeData("Parachute", 0, new int[] { 100, 150, 250 }, "Parachute des minions n'importe où sur une lane.\nLvl1 : un magicien\nLvl2 : un magicien + un tank\nLvl3 : un magicien + un tank + un healer.");
        opCodeToNode[1211] = new NodeData("Salvage", 0, new int[] { 100, 150, 250 });
        opCodeToNode[1212] = new NodeData("Wizard", 0, new int[] { 100, 150, 250 }, "Rajoute des cartes Magiciens au Minion Manager.\nLvl1 : cartes++\nLvl2 : cartes++, gain d'un sort de protection pour les héros\nLvl3 : cartes++, les attaques de base ralentissent les ennemis.");
        opCodeToNode[1213] = new NodeData("Bowman", 0, new int[] { 100, 150, 250 }, "Rajoute des cartes Archers au Minion Manager.\nLvl1 : cartes++\nLvl2 : cartes++, les attaques rebondissent 1 fois\nLvl3 : cartes++, les attaques font des dégâts supplémentaires aux Héros.");
        opCodeToNode[1214] = new NodeData("Footy", 0, new int[] { 100, 150, 250 }, "Rajoute des cartes Fantassins au Minion Manager.\nLvl1 : cartes++\nLvl2 : cartes++, les attaques de base font des dégâts en AOE\nLvl3 : cartes++, les footies ne rapportent plus d'or aux ennemis lorsqu'ils sont tués.");



#if UNITY_EDITOR
        // some checks to avoid useless debugging
        foreach (var v in opCodeToNode)
        {
            if (((v.Value.rank == 3 || v.Value.rank == 2) && v.Value.goldPerRank.Length != 1) || 
                (v.Value.rank == 1 && v.Value.goldPerRank.Length != 2) ||
                (v.Value.rank == 0 && v.Value.goldPerRank.Length != 3))
                Debug.LogError("The rank of the node " + v.Value.prefabName + "don't match the gold per rank.\nrank 0 -> goldPerRank.Length = 3, rank 1 -> goldPerRank.Length = 2, rank 2 -> goldPerRank.Length = 1");
        }

#endif
    }

    [System.Serializable]
    public struct NodeData
    {
        public string prefabName;
        public int rank;
        public int[] goldPerRank;

        public string description;

        public NodeData(string prefabName_, int rank_, int[] goldPerRank_, string description_ = "")
        {
            prefabName = prefabName_;
            rank = rank_;
            goldPerRank = goldPerRank_;
            description = description_;
        }
    }
}
