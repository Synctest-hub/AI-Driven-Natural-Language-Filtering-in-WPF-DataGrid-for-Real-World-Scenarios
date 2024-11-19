using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartFilterPredicates_Demo
{
    public class SemanticKernelAI
    {
        /// <summary>
        /// Holds the IChatCompletionService instance.
        /// </summary>
        private IChatCompletionService chatCompletionService;

        /// <summary>
        /// Holds the Kernel instance.
        /// </summary>
        private Kernel kernel;

        /// <summary>
        /// Method to get the response from OpenAI using the semantic kernel
        /// </summary>
        /// <param name="prompt">Prompt for the system message</param>
        /// <returns>The AI results.</returns>
        public async Task<string> GetResponseFromGPT(string prompt)
        {
            try
            {
                // Create a kernel with Azure OpenAI chat completion
                var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion("Model Name", "EndPoint Link", "Key");

                // Build the kernel
                kernel = builder.Build();

                chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

                var history = new ChatHistory();
                history.AddSystemMessage(prompt);

                // Set the execution settings
                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings();
                openAIPromptExecutionSettings.ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions;

                // Get the response from the AI 
                var result = await chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

                return result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                // Return an empty string if an exception occurs
                return string.Empty;
            }
        }

       
    }
}
