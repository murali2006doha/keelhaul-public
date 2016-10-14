
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KrakenStats : MonoBehaviour
{

    [System.Serializable]
    public struct krakenStage
    {
        public string stageName;
        public float max_health;
        public float shootDelay;
        public float damage;
        public float moveSpeed;
        public float emergeTime;
        public float deaccelaration;
        public float turnSpeed;
		public float emergingTurnSpeed;
		public float emergingMoveSpeed;
		public float emergingMaxVelocity;
        public float submergedTurnSpeed;
        public float submergedMoveSpeed;
        public float submergedMaxVelocity;
        public float maxVelocity;
        public float lineAttackVelocity;
        public float headbashChargeVelocity;
        public float maxHeadbashChargeVelocity;
        public float weight;
        public List<String> moves;
        public KrakenAnimator animator;
    }

    public float max_health = 7;
    public float shootDelay = 0.1f;
    public float damage = 1;
    public float moveSpeed = 10f;
    public float emergeTime = 1.2f;
    public float deaccelaration = 4f;
    public float turnSpeed = 160f;
    public float submergedTurnSpeed = 320f;
    public float submergedMoveSpeed = 10f;
    public float submergedMaxVelocity = 2.7f;
    public float maxVelocity = 2f;
    public float sideSteppingVelocity = 9f;
    public float sideSteppingDeacceleration = 18f;
    public float lineAttackVelocity = 5f;

    [SerializeField]
    public krakenStage[] stages;

    internal bool canPerformAction(string name, int currentStage)
    {
        if (stages[currentStage].moves.Contains(name)) {
            return true;
        }
        return false;
    }
}