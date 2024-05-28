namespace PokemonReviewApp.Models
{
    public class PokemonOwner
    {
        public int PokemonId { get; set; }

        public int OwnerId { get; set; }

        public  virtual Pokemon Pokemon { get; set; }

        public virtual Owner Owner { get; set; }


    }
}
