using System;

namespace ProjectIO.model
{
    public static class CurrentWorker
    {

        // Prywatne statyczne pole przechowujące instancję Singletona
        private static Worker _instance;

        // Obiekt synchronizacji do zapewnienia bezpieczeństwa wątkowego
        private static readonly object _lock = new object();

        // Publiczna metoda statyczna umożliwiająca dostęp do Singletona
        public static Worker GetInstance()
        {
            
            if (_instance == null)
            {
                //Utworzenie obiektu CurrentWorker eliminuje cykl instnienia CurrentUser
                CurrentUser.EndInstance();
                _instance = new Worker();
            }
            return _instance;
        }

        // Publiczna metoda statyczna umożliwiająca przypisanie istniejącego obiektu
        public static void SetInstance(Worker worker)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker));

            lock (_lock) // Blok synchronizowany dla bezpieczeństwa wątkowego
            {
                _instance = worker;
            }
        }

        public static void EndInstance()
        {
            _instance = null;
        }
    }
}

