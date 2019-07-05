namespace UPS.ServicesDataRepository.Common
{
    public static class Enums
    {
       public enum ATStatus
        {
            Uploaded = 0,
            Curated = 1,
            Translated = 2,
            Completed = 3,
            Inactive =4
        }

        public enum WorkflowStatus
        {
            Created = 0,
            InProgress= 1,
            Completed = 2
        }
    }

}
