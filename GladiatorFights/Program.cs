using System;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            





 




            static void CreateFighter(Fighter fighter)
            {

            }
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

        public virtual void TakeDamage(float damage) => Health -= damage;

        public abstract void DealingDamage(Fighter fighter);
    }

    class Warrior : Fighter
    {
        private float _chanceDoubleDamage;

        public Warrior(float health, float damage, float chanceDoubleDamage) : base(health, damage)
        {
            _chanceDoubleDamage = chanceDoubleDamage;
        }

        public override void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(GetCalculatedDamage());
        }

        private float GetCalculatedDamage()
        {
            Random random = new Random();
            double randomValue = random.NextDouble();

            if (randomValue < _chanceDoubleDamage)
            {
                Console.WriteLine("Удваиваем уроне!");
                return Damage * 2;
            }
            else
            {
                return Damage;
            }
        }
    }

    class Knight : Fighter
    {
        private int _attackNumber = 0;
        private int _attackNumberStrengthen;

        public Knight(float health, float damage, int attackNumber, int attackNumberStrengthen) : base(health, damage)
        {
            _attackNumber = attackNumber;
            _attackNumberStrengthen = attackNumberStrengthen;
        }

        public override void DealingDamage(Fighter fighter)
        {
            _attackNumber++;

            if (_attackNumber % _attackNumberStrengthen == 0)
                fighter.TakeDamage(Damage);

            fighter.TakeDamage(Damage);
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

        public override void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            _currentRage += damage;

            if (_currentRage >= _maxRage)
            {
                Treatment();
                _currentRage = 0;
            }
        }

        private void Treatment() => Health += _increasedHealth;
    }

    class Mage : Fighter
    {
        private float _mana;
        private float _manaCost;
        private float _damageFireBall;

        public Mage(float health, float damage, float damageFireBall) : base(health, damage)
        {
            _damageFireBall = damageFireBall;

            if (damage >= _damageFireBall)
                _damageFireBall = damage + 1;
        }

        public override void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
            fighter.TakeDamage(GetDamageFireBall());
        }

        private float GetDamageFireBall()
        {
            if (_mana >= _manaCost)
            {
                _mana -= _manaCost;
                return _damageFireBall;
            }
            else
            {
                return 0;
            }
        }
    }

    class Rogue : Fighter
    {
        private float _chanceEvasion;

        public Rogue(float health, float damage, float chanceEvasion) : base(health, damage)
        {
            _chanceEvasion = chanceEvasion;
        }

        public override void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public override void TakeDamage(float damage)
        {
            if (TryEvasion() == false)
                base.TakeDamage(damage);
        }

        private bool TryEvasion()
        {
            Random random = new Random();
            double randomValue = random.NextDouble();

            return randomValue < _chanceEvasion;
        }
    }
}