using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IStorage _storage;
        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _storage = storage;
            _logger = logger;
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

            string textKey = Constants.TextKeyPrefix + id;
            _storage.StoreValue(textKey, text);

            string rankKey = Constants.RankKeyPrefix + id;
            var rank = GetRank(text);
            _storage.StoreValue(rankKey, rank.ToString());

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

        private double GetRank(string text)
        {
            int nonalphaCounter = 0;
            foreach (var ch in text)
            {
                if (!Char.IsLetter(ch))
                {
                    nonalphaCounter++;
                }
            }
            return Convert.ToDouble(nonalphaCounter) / Convert.ToDouble(text.Length);
        }
    }
}
