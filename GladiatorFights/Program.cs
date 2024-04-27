using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Services;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool _isWork = true;
            bool _isFight = true;

            Arena arena = new Arena();
            Fighter fighterFirst = null;
            Fighter fighterSecond = null;

            while (_isWork)
            {
                Console.WriteLine("Привет игрок! Ты на гладиаторских боях! Ниже приведен список бойцов. Выбирай пару для боя!");

                fighterFirst = arena.EnterFighters(); 
                Console.WriteLine();
                fighterSecond = arena.EnterFighters();

                Console.Clear();

                _isFight = true;

                while (_isFight)
                {
                    if (fighterFirst == null || fighterSecond == null)
                        break;

                    fighterFirst.DealingDamage(fighterSecond);
                    fighterSecond.ShowStats(fighterSecond.GetName);
                    fighterSecond.DealingDamage(fighterFirst);
                    fighterFirst.ShowStats(fighterFirst.GetName);

                    if (fighterFirst.GetHealth <= 0 && fighterSecond.GetHealth <= 0)
                    {
                        Console.WriteLine("\nНИЧЬЯ");
                        _isFight = false;
                    }
                    else
                    {
                        if (fighterFirst.GetHealth <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + fighterSecond.GetName);
                            _isFight = false;
                        }
                        else if (fighterSecond.GetHealth <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + fighterFirst.GetName);
                            _isFight = false;
                        }
                    }
                }
                _isWork = false;
                Console.ReadKey();
            }
        }
    }

    class Arena
    {
        private const int _commandEnterWarrior = 1;
        private const int _commandEnterKnight = 2;
        private const int _commandEnterBarbarian = 3;
        private const int _commandEnterMage = 4;
        private const int _commandEnterRogue = 5;

        private float _helthWarrior = 100;
        private float _damageWarrior = 10;
        private float _chanceDoubleDamageWarrior = 0.7f;
        private float _damageEnhancement = 2;

        private float _helthKnight = 100;
        private float _damageKnight = 10;
        private int _attackNumberStrengthenKnight = 3;

        private float _helthBarbarian = 100;
        private float _damageBarbarian = 10;
        private float _maxRageBarbarian = 50;
        private float _increasedHealthBarbarian = 10;

        private float _helthMage = 100;
        private float _damageMage = 10;
        private float _manaMage = 15;
        private float _manaCosMage = 6;
        private float _damageFireBallMage = 12;

        private float _helthRogue = 100;
        private float _damageRogue = 10;
        private float _chanceEvasionRogue = 0.7f;
        private List<Fighter> _fighters;

        public Arena()
        {
            _fighters = new List<Fighter>() {
            new Warrior(_helthWarrior, _damageWarrior, _chanceDoubleDamageWarrior, _damageEnhancement),
            new Knight(_helthKnight, _damageKnight, _attackNumberStrengthenKnight),
            new Barbarian(_helthBarbarian, _damageBarbarian, _maxRageBarbarian, _increasedHealthBarbarian),
            new Mage(_helthMage, _damageMage, _manaMage, _manaCosMage, _damageFireBallMage),
            new Rogue(_helthRogue, _damageRogue, _chanceEvasionRogue)
            };
        }

        public Fighter EnterFighters()
        {
            Console.WriteLine($"{_commandEnterWarrior} - Воин");
            Console.WriteLine($"{_commandEnterKnight} - Рыцарь");
            Console.WriteLine($"{_commandEnterBarbarian} - Варвар");
            Console.WriteLine($"{_commandEnterMage} - Маг");
            Console.WriteLine($"{_commandEnterRogue} - Разбойник");

            Console.Write("\nВыбери бойца:");

            if (int.TryParse(Console.ReadLine(), out int inputUser))
            {
                if (inputUser > 0 && _fighters.Count >= inputUser)
                {
                    return _fighters[inputUser-1];
                }
                else
                {
                    Console.WriteLine("Такой команды не существует!");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Введенный не корректные данные!");
                return null;
            }
        }
    }

    class Fighter
    {
        protected float Health;
        protected float Damage;
        protected string Name;

        public Fighter(float health, float damage)
        {
            Health = health;
            Damage = damage;
        }

        public float GetHealth => Health;
        public string GetName => Name;

        public virtual void TakeDamage(float damage) => Health -= damage;

        public virtual void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public void ShowStats(string nameFighter)
        {
            Console.WriteLine(nameFighter + " колличество жизней: " + Health + " наносимый урон: " + Damage);
        }
    }

    class Warrior : Fighter
    {
        private float _chanceDoubleDamage;
        private float _damageEnhancement;

        public Warrior(float health, float damage, float chanceDoubleDamage, float damageEnhancement) : base(health, damage)
        {
            _chanceDoubleDamage = chanceDoubleDamage;
            _damageEnhancement = damageEnhancement;
            Name = "Воин";
        }

        public override void DealingDamage(Fighter fighter)
        {
            fighter.TakeDamage(GetCalculatedDamage());
        }

        private float GetCalculatedDamage()
        {
            double randomValue = UserUtils.GenerateRandomNumber();

            if (randomValue < _chanceDoubleDamage)
            {
                Console.WriteLine("\nУдваиваем уроне!\n");
                return Damage * _damageEnhancement;
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

        public Knight(float health, float damage, int attackNumberStrengthen) : base(health, damage)
        {
            _attackNumberStrengthen = attackNumberStrengthen;
            Name = "Рыцарь";
        }

        public override void DealingDamage(Fighter fighter)
        {
            _attackNumber++;
            base.DealingDamage(fighter);

            if (_attackNumber % _attackNumberStrengthen == 0)
            {
                Console.WriteLine("\nНаносим второй раз урон!\n");
                fighter.TakeDamage(Damage);
            }
        }
    }

    class Barbarian : Fighter
    {
        private float _currentRage;
        private float _maxRage;
        private float _increasedHealth;

        public Barbarian(float health, float damage, float maxRage, float increasedHealth) : base(health, damage)
        {
            _maxRage = maxRage;
            _increasedHealth = increasedHealth;
            Name = "Варвар";
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            _currentRage += damage;

            if (_currentRage >= _maxRage)
            {
                Treatment();
                Console.WriteLine("\nЛечение!\n");
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

        public Mage(float health, float damage, float mana, float manaCost, float damageFireBall) : base(health, damage)
        {
            _mana = mana;
            _manaCost = manaCost;
            _damageFireBall = damageFireBall;

            if (damage >= _damageFireBall)
                _damageFireBall = damage + 1;

            Name = "Маг";
        }

        public override void DealingDamage(Fighter fighter)
        {
            base.DealingDamage(fighter);
            fighter.TakeDamage(GetDamageFireBall());
        }

        private float GetDamageFireBall()
        {
            if (_mana >= _manaCost)
            {
                Console.WriteLine("\nЗаклинание:огненный шар!\n");
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
            Name = "Разбойник";
        }

        public override void TakeDamage(float damage)
        {
            if (TryEvasion() == false)
            {
                base.TakeDamage(damage);
            }
            else
            {
                Console.WriteLine("\nУклонение!\n");
            }
        }

        private bool TryEvasion()
        {
            double randomValue = UserUtils.GenerateRandomNumber();

            return randomValue < _chanceEvasion;
        }
    }

    class UserUtils
    {
        static Random s_random;

        public static double GenerateRandomNumber()
        {
            s_random = new Random();
            return s_random.NextDouble();
        }
    }
}
