using StoreOfBuild.Domain.Dto;

namespace StoreOfBuild.Domain.Products
{
    public class CategoryStorer
    {
        public readonly IRepository<Category> categoryRepository;

        public CategoryStorer(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public void Store(int id, string name)
        {
            var category = this.categoryRepository.GetById(id);

            if (category == null)
            {
                category = new Category(name);
                this.categoryRepository.Save(category);
            }
            else
            {
                category.Update(name);
            }
        }
    }
}