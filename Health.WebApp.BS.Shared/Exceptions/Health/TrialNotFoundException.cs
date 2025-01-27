namespace HealthManager.WebApp.BS.Shared.Exceptions.Health
{
    public class TrialNotFoundException : NotFoundException
    {
        public TrialNotFoundException(int healthId) : base($"The Health with id: {healthId} doesn't exist in the database.")
        {

        }

        public TrialNotFoundException(string healthIndex) : base($"The Health with index: {healthIndex} doesn't exist in the database.")
        {

        }
    }
}
