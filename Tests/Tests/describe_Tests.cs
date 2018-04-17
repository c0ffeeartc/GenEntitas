using NSpec;

public class describe_Tests : nspec
{
	private void when_testing()
	{
		it["works"] = () =>
		{
			true.should_be_true(  );
		};
	}
}
