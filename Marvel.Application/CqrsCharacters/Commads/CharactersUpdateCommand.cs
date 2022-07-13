namespace Marvel.Application.CqrsCharacters.Commads
{
    public class CharactersUpdateCommand : CharactersCommand
    {
        public int Id { get; set; }
    }
}
