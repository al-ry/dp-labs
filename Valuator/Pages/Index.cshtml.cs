using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;
        private readonly IPublisher _publisher;
        public IndexModel(ILogger<IndexModel> logger, IStorage storage, IPublisher publisher)
        {
            _storage = storage;
            _logger = logger;
            _publisher = publisher;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return Redirect($"summary");
            }

            string id = Guid.NewGuid().ToString();
            string similarityKey = Constants.SimilarityKeyPrefix + id;
            var similarity = GetSimilarity(text);
            _storage.StoreValue(similarityKey, similarity.ToString());
            byte[] iddata = Encoding.UTF8.GetBytes(id);
            //_messageBroker.Publish(Constants.SimilarityCalculatedEventName, iddata);

            string textKey = Constants.TextKeyPrefix + id;
            _storage.StoreValue(textKey, text);
            _storage.StoreTextToSet(text);

            string rankKey = Constants.RankKeyPrefix + id;
            byte[] data = Encoding.UTF8.GetBytes(id);
            _publisher.Publish(Constants.RankCalculatorEventName, data);

            return Redirect($"summary?id={id}");     
        }

        private double GetSimilarity(string text)
        {
            if (_storage.FindText(text))
            {
                return 1d;
            }
            return 0d;
        }
    }
}
