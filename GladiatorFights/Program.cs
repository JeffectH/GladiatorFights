using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();

            arena.Fight();
        }
    }

    class Arena
    {
        private bool _isWork = true;
        private bool _isFight = true;
        private bool _isWinner = true;

        private List<Fighter> _fighters;
        private Fighter _fighterFirst;
        private Fighter _fighterSecond;

        public void Fight()
        {
            while (_isWork)
            {
                Console.WriteLine("Привет игрок! Ты на гладиаторских боях! Ниже приведен список бойцов. Выбирай пару для боя!");

                _fighterFirst = EnterFighter();
                Console.WriteLine();
                _fighterSecond = EnterFighter();

                Console.Clear();

                if (_fighterFirst == null || _fighterSecond == null)
                {
                    _isFight = false;
                    _isWinner = false;
                    Console.WriteLine("\nБой не состоиться! Бойцы были выбраны не верно!");
                }

                while (_isFight)
                {
                    _fighterFirst.AttackEnemy(_fighterSecond);
                    Console.Write("Боец 1: ");
                    _fighterSecond.ShowStats(_fighterSecond.GetName());
                    _fighterSecond.AttackEnemy(_fighterFirst);
                    Console.Write("Боец 2: ");
                    _fighterFirst.ShowStats(_fighterFirst.GetName());

                    if (_fighterFirst.Health <= 0 || _fighterSecond.Health <= 0)
                        _isFight = false;

                }

                if (_isWinner)
                {
                    if (_fighterFirst.Health <= 0 && _fighterSecond.Health <= 0)
                    {
                        Console.WriteLine("\nНИЧЬЯ");
                    }
                    else
                    {
                        if (_fighterFirst.Health <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + _fighterSecond.GetName());
                        }
                        else if (_fighterSecond.Health <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + _fighterFirst.GetName());
                        }
                    }
                }

                _isWork = false;
                Console.ReadKey();
            }
        }

        private Fighter EnterFighter()
        {
            _fighters = new List<Fighter>() {
            new Warrior(100, 11, 0.7f, 2),
            new Knight(100, 12, 3),
            new Barbarian(100, 9, 50, 10),
            new Mage(100, 8, 15, 6, 12),
            new Rogue(100, 9.5f, 0.3f)
            };

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.WriteLine($"{i} - {_fighters[i].GetName()}");
            }

            Console.Write("\nВыбери бойца:");

            if (int.TryParse(Console.ReadLine(), out int inputUser))
            {
                if (inputUser > 0 && _fighters.Count > inputUser)
                {
                    return _fighters[inputUser];
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
        protected float Damage;
        protected string Name;

        public Fighter(float health, float damage)
        {
            Health = health;
            Damage = damage;
        }

        public float Health { get; private set; }

        public string GetName() => Name;

        public virtual void TakeDamage(float damage) => Health -= damage;

        public virtual void AttackEnemy(Fighter fighter) => fighter.TakeDamage(Damage);

        public void ShowStats(string nameFighter) => Console.WriteLine(nameFighter + " колличество жизней: " + Health + " наносимый урон: " + Damage);

        protected void RestoringHealth(float amountExtraHealth) => Health += amountExtraHealth;
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

        public override void AttackEnemy(Fighter fighter)
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

        public override void AttackEnemy(Fighter fighter)
        {
            _attackNumber++;
            base.AttackEnemy(fighter);

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

        private void Treatment() => RestoringHealth(_increasedHealth);
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

        public override void AttackEnemy(Fighter fighter)
        {
            base.AttackEnemy(fighter);
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
            if (CanDodge() == false)
            {
                base.TakeDamage(damage);
            }
            else
            {
                Console.WriteLine("\nУклонение!\n");
            }
        }

        private bool CanDodge()
        {
            double randomValue = UserUtils.GenerateRandomNumber();

            return randomValue < _chanceEvasion;
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static double GenerateRandomNumber() => s_random.NextDouble();
    }
}
