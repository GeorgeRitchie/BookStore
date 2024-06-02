namespace Application.Common.Models
{
	public class EntityCollection<TEntity> where TEntity : class
	{
		public List<TEntity> EntityList { get; set; }
	}
}
