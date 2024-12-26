using System;

namespace ProjectIO.model
{
    public static class CurrentPerson
    {

        // Prywatne statyczne pole przechowujące instancję Singletona
        private static Person _instance;

        // Obiekt synchronizacji do zapewnienia bezpieczeństwa wątkowego
        private static readonly object _lock = new object();

        // Publiczna metoda statyczna umożliwiająca dostęp do Singletona
        public static Person GetInstance()
        {
            return _instance;
        }

        // Publiczna metoda statyczna umożliwiająca przypisanie istniejącego obiektu
        public static void SetInstance(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            lock (_lock) // Blok synchronizowany dla bezpieczeństwa wątkowego
            {
                
                _instance = person;
            }
        }

        public static void EndInstance()
        {
            _instance = null;
        }
    }
}

