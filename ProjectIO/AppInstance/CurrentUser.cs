using System;

namespace ProjectIO.model
{
    public static class CurrentUser
    {
        
        // Prywatne statyczne pole przechowujące instancję Singletona
        private static User _instance;

        // Obiekt synchronizacji do zapewnienia bezpieczeństwa wątkowego
        private static readonly object _lock = new object();

        // Publiczna metoda statyczna umożliwiająca dostęp do Singletona
        public static User GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock) // Blok synchronizowany dla bezpieczeństwa wątkowego
                {
                    if (_instance == null)
                    {
                        _instance = new User();
                    }
                }
            }

            return _instance;
        }

        // Publiczna metoda statyczna umożliwiająca przypisanie istniejącego obiektu
        public static void SetInstance(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            lock (_lock) // Blok synchronizowany dla bezpieczeństwa wątkowego
            {
                _instance = user;
            }
        }
    }
}

