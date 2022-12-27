using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveFrequentQuestionsRequest
	{
		public FrequentQuestions frequentQuestions { get; set; }
		public int familyId { get; set; }
	}
}
