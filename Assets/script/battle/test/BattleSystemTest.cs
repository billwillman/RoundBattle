﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RoundBattle;
using RoundBattle.Record;
using RoundBattle.Command;

namespace RoundBattle.Test {

    [RequireComponent(typeof(BattleSystem))]
    public class BattleSystemTest : MonoBehaviour {
        private BattleSystem m_Sys = null;

        private void Start() {
            m_Sys = GetComponent<BattleSystem>();
            Init();
        }

        void Init() {
            BattleRecord record = new BattleRecord();

            var fighters = new List<RecordFighter>();

            RecordFighter fighter = new RecordFighter();
            fighter.isMySelf = true;
            fighter.model = "role_lingnv4";
            fighter.serverId = 10;
            fighter.isPlayer = true;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "role_lingnv4";
            fighter.serverId = 11;
            fighter.isPlayer = true;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "role_lingnv4";
            fighter.serverId = 12;
            fighter.isPlayer = true;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "role_lingnv4";
            fighter.serverId = 13;
            fighter.isPlayer = true;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "role_lingnv4";
            fighter.serverId = 14;
            fighter.isPlayer = true;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 15;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 16;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 17;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 18;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 19;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            // 敌方
            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 0;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 1;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 2;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 3;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 4;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 5;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 6;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 7;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 8;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            fighter = new RecordFighter();
            fighter.isMySelf = false;
            fighter.model = "pet_jiangshi";
            fighter.serverId = 9;
            fighter.isPlayer = false;
            fighters.Add(fighter);

            record.fighters = fighters;

            m_Sys.LoadBattleRecord(record);
        }
    }

}
