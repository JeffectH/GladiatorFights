using System;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    abstract class Fighter
    {
        protected float Health;
        protected float Damage;

        public Fighter(float health, float damage)
        {
            Health = health;
            Damage = damage;
        }

        public abstract void TakeDamage(float damage);
    }

    class Warrior : Fighter
    {
        private float _chanceDoubleDamage;

        public Warrior(float health, float damage, float chanceDoubleDamage) : base(health, damage)
        {
            _chanceDoubleDamage = chanceDoubleDamage;
        }

        public override void TakeDamage(float damage)
        {

        }
    }

    class Knight : Fighter
    {
        private int _attackNumber;
        private float _strengtheningAttack;

        public Knight(float health, float damage, int attackNumber, float strengtheningAttack) : base(health, damage)
        {
            _attackNumber = attackNumber;
            _strengtheningAttack = strengtheningAttack;
        }

        public override void TakeDamage(float damage)
        {
        }
    }

    class Barbarian : Fighter
    {
        private float _currentRage;
        private float _maxRage;
        private float _increasedHealth;

        public Barbarian(float health, float damage, float currentRage, float maxRage, float increasedHealth) : base(health, damage)
        {
            _currentRage = currentRage;
            _maxRage = maxRage;
            _increasedHealth = increasedHealth;
        }

        public override void TakeDamage(float damage)
        {
        }

        private void Treatment()
        {

        }
    }

    class Mage : Fighter
    {
        private float _mana;
        private float _increasedHealth;
        private float _damageFireBall;

        public Mage(float health, float damage, float increasedHealth, float damageFireBall) : base(health, damage)
        {
            _increasedHealth = increasedHealth;
            _damageFireBall = damageFireBall;

            if (damage >= _damageFireBall)
                _damageFireBall = damage + 1;
        }

        public override void TakeDamage(float damage)
        {
        }

        private void FireBall()
        {

        }
    }

    class Rogue : Fighter
    {
        private float _chanceEvasion;

        public Rogue(float health, float damage, float chanceEvasion) : base(health, damage)
        {
            _chanceEvasion = chanceEvasion;
        }

        public override void TakeDamage(float damage)
        {
        }
    }


}
