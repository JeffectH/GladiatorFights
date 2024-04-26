using System;
using System.Diagnostics;

namespace GladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int CommandEnterWarrior = 1;
            const int CommandEnterKnight = 2;
            const int CommandEnterBarbarian = 3;
            const int CommandEnterMage = 4;
            const int CommandEnterRogue = 5;

            float helthWarrior = 100;
            float damageWarrior = 10;
            float chanceDoubleDamageWarrior = 0.3f;

            float helthKnight = 100;
            float damageKnight = 10;
            int attackNumberStrengthenKnight = 3;

            float helthBarbarian = 100;
            float damageBarbarian = 10;
            float maxRageBarbarian = 50;
            float increasedHealthBarbarian = 10;

            float helthMage = 100;
            float damageMage = 10;
            float manaMage = 15;
            float manaCosMage = 6;
            float damageFireBallMage = 12;

            float helthRogue = 100;
            float damageRogue = 10;
            float chanceEvasionRogue = 0.3f;

            bool _isWork = true;
            bool _isFight = true;

            Fighter fighterFirst = null;
            Fighter fighterSecond = null;

            while (_isWork)
            {
                Console.WriteLine("Привет игрок! Ты на гладиаторских боях! Ниже приведен список бойцов. Выбирай пару для боя!");
                EnterFighters();
                Console.WriteLine();
                EnterFighters();

                Console.Clear();

                _isFight = true;

                while (_isFight)
                {
                    fighterFirst.DealingDamage(fighterSecond);
                    fighterSecond.ShowStats(fighterSecond.GetName);
                    fighterSecond.DealingDamage(fighterFirst);
                    fighterFirst.ShowStats(fighterFirst.GetName);

                    if (fighterFirst.GetHealth <= 0 && fighterSecond.GetHealth <= 0)
                    {
                        Console.WriteLine("НИЧЬЯ");
                        _isFight = false;
                    }
                    else
                    {
                        if (fighterFirst.GetHealth <= 0)
                        {
                            Console.WriteLine("Победа за " + fighterSecond.GetName);
                            _isFight = false;
                        }
                        else if (fighterSecond.GetHealth <= 0)
                        {
                            Console.WriteLine("Победа за " + fighterFirst.GetName);
                            _isFight = false;
                        }
                    }
                }
                Console.WriteLine("\nБой окончен!");
                _isWork = false;
                Console.ReadKey();
            }

            void EnterFighters()
            {
                Console.WriteLine($"{CommandEnterWarrior} - Воин");
                Console.WriteLine($"{CommandEnterKnight} - Рыцарь");
                Console.WriteLine($"{CommandEnterBarbarian} - Варвар");
                Console.WriteLine($"{CommandEnterMage} - Маг");
                Console.WriteLine($"{CommandEnterRogue} - Разбойник");

                Console.Write("\nВыбери бойца:");

                if (int.TryParse(Console.ReadLine(), out int inputUser))
                {
                    switch (inputUser)
                    {
                        case CommandEnterWarrior:
                            CreateFighter(new Warrior(helthWarrior, damageWarrior, chanceDoubleDamageWarrior));
                            break;

                        case CommandEnterKnight:
                            CreateFighter(new Knight(helthKnight, damageKnight, attackNumberStrengthenKnight));
                            break;

                        case CommandEnterBarbarian:
                            CreateFighter(new Barbarian(helthBarbarian, damageBarbarian, maxRageBarbarian, increasedHealthBarbarian));
                            break;

                        case CommandEnterMage:
                            CreateFighter(new Mage(helthMage, damageMage, manaMage, manaCosMage, damageFireBallMage));
                            break;

                        case CommandEnterRogue:
                            CreateFighter(new Rogue(helthRogue, damageRogue, chanceEvasionRogue));
                            break;

                        default:
                            Console.WriteLine("Такой команды не существует!");
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("Введенный не корректные данные!");
                }
            }

            void CreateFighter(Fighter fighter)
            {
                if (fighterFirst == null)
                {
                    fighterFirst = fighter;
                    return;
                }

                if (fighterSecond == null)
                {
                    fighterSecond = fighter;
                }
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

        public Warrior(float health, float damage, float chanceDoubleDamage) : base(health, damage)
        {
            _chanceDoubleDamage = chanceDoubleDamage;
            Name = "Воин";
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
                Console.WriteLine("\nУдваиваем уроне!\n");
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
            Random random = new Random();
            double randomValue = random.NextDouble();

            return randomValue < _chanceEvasion;
        }
    }
}