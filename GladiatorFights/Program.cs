using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Services;

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
        private const int CommandEnterWarrior = 1;
        private const int CommandEnterKnight = 2;
        private const int CommandEnterBarbarian = 3;
        private const int CommandEnterMage = 4;
        private const int CommandEnterRogue = 5;

        private bool _isWork = true;
        private bool _isFight = true;

        private List<Fighter> _fighters;
        private Fighter _fighterFirst;
        private Fighter _fighterSecond;

        private int _beforeInputUser;

        public Arena()
        {
            _fighters = new List<Fighter>() {
            new Warrior(100, 11, 0.7f, 2),
            new Knight(100, 12, 3),
            new Barbarian(100, 9, 50, 10),
            new Mage(100, 8, 15, 6, 12),
            new Rogue(100, 9.5f, 0.3f)
            };
        }

        public void Fight()
        {
            while (_isWork)
            {
                Console.WriteLine("Привет игрок! Ты на гладиаторских боях! Ниже приведен список бойцов. Выбирай пару для боя!");

                _fighterFirst = EnterFighters();
                Console.WriteLine();
                _fighterSecond = EnterFighters();

                Console.Clear();

                _isFight = true;

                while (_isFight)
                {
                    if (_fighterFirst == null || _fighterSecond == null)
                        break;

                    _fighterFirst.AttackEnemy(_fighterSecond);
                    Console.Write("Боец 1: ");
                    _fighterSecond.ShowStats(_fighterSecond.GetName());
                    _fighterSecond.AttackEnemy(_fighterFirst);
                    Console.Write("Боец 2: ");
                    _fighterFirst.ShowStats(_fighterFirst.GetName());

                    if (_fighterFirst.GetHealth() <= 0 && _fighterSecond.GetHealth() <= 0)
                    {
                        Console.WriteLine("\nНИЧЬЯ");
                        _isFight = false;
                    }
                    else
                    {
                        if (_fighterFirst.GetHealth() <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + _fighterSecond.GetName());
                            _isFight = false;
                        }
                        else if (_fighterSecond.GetHealth() <= 0)
                        {
                            Console.WriteLine("\nПобеда за " + _fighterFirst.GetName());
                            _isFight = false;
                        }
                    }
                }
                _isWork = false;
                Console.ReadKey();
            }
        }

        private Fighter EnterFighters()
        {
            Console.WriteLine($"{CommandEnterWarrior} - Воин");
            Console.WriteLine($"{CommandEnterKnight} - Рыцарь");
            Console.WriteLine($"{CommandEnterBarbarian} - Варвар");
            Console.WriteLine($"{CommandEnterMage} - Маг");
            Console.WriteLine($"{CommandEnterRogue} - Разбойник");

            Console.Write("\nВыбери бойца:");

            if (int.TryParse(Console.ReadLine(), out int inputUser))
            {
                if (inputUser > 0 && _fighters.Count >= inputUser)
                {
                    if(_beforeInputUser==inputUser)
                        return _fighters[inputUser - 1].Clone() as Fighter;

                    return _fighters[inputUser - 1];
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

    class Fighter: ICloneable
    {
        protected float Health;
        protected float Damage;
        protected string Name;

        public Fighter(float health, float damage)
        {
            Health = health;
            Damage = damage;
        }

        public float GetHealth() => Health;
        public string GetName() => Name;

        public virtual void TakeDamage(float damage) => Health -= damage;

        public virtual void AttackEnemy(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public void ShowStats(string nameFighter)
        {
            Console.WriteLine(nameFighter + " колличество жизней: " + Health + " наносимый урон: " + Damage);
        }

        public object Clone()
        {
            Fighter clone = new Fighter(Health, Damage);
            clone.Health = Health;
            clone.Damage = Damage;

            return clone;
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
        private static Random s_random = new Random();

        public static double GenerateRandomNumber() => s_random.NextDouble();
    }
}
