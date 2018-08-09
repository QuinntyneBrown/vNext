namespace IntegrationTests
{
    public class NoteScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Notes = "api/notes";

            public static string GetById(int id)
            {
                return $"api/notes/{id}";
            }

            public static string GetNoteIdByNote(string note)
            {
                return $"api/notes/id/{note}";
            }
        }

        public static class Post
        {
            public static string Notes = "api/notes";
        }
    }
}
