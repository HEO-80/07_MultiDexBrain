using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Web3;
using Nethereum.Contracts;

class Program
{
    // --- 1. CONFIGURACIÓN ---
    // PEGA AQUÍ TU URL DE ALCHEMY (La misma que usas en Foundry)
    static string rpcUrl = "https://eth-mainnet.g.alchemy.com/v2/xWz8OndQ0A-LfRt8sdxB3";
    
    // Direcciones de Contratos en Mainnet
    static string SUSHI_ROUTER = "0xd9e1cE17f2641f24aE83637ab66a2cca9C378B9F";
    static string UNI_QUOTER = "0xb27308f9F90D607463bb33eA1BeBb41C27CE5AB6"; // Quoter V1
    
    static string WETH = "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2";
    static string DAI = "0x6B175474E89094C44Da98b954EedeAC495271d0F";

    static async Task Main(string[] args)
    {
        Console.WriteLine("🦈 INICIANDO CEREBRO MULTI-DEX...");
        var web3 = new Web3(rpcUrl);

        try
        {
            // Verificamos conexión consultando el bloque actual
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            Console.WriteLine($"✅ Conectado a Ethereum Mainnet. Bloque: {blockNumber}");
            
            while (true)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("🔎 Buscando precios para 1 WETH -> DAI...");

                // 1. Obtener precio de SUSHISWAP (V2)
                decimal sushiPrice = await GetSushiPrice(web3);
                Console.WriteLine($"🍣 SushiSwap: {sushiPrice:F2} DAI");

                // 2. Obtener precio de UNISWAP (V3)
                decimal uniPrice = await GetUniPrice(web3);
                Console.WriteLine($"🦄 Uniswap V3: {uniPrice:F2} DAI");

                // 3. Calcular Diferencia (SPREAD)
                decimal diff = uniPrice - sushiPrice;
                Console.WriteLine($"📊 Diferencia: {diff:F2} DAI");

                if (diff > 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                
                Console.WriteLine($"👉 {(diff > 0 ? "Uniswap paga más" : "SushiSwap paga más")}");
                Console.ResetColor();

                // Esperar 5 segundos antes de volver a mirar
                await Task.Delay(5000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR CRÍTICO: {ex.Message}");
        }
    }

    // --- FUNCIONES AUXILIARES PARA LEER PRECIOS ---

    static async Task<decimal> GetSushiPrice(Web3 web3)
    {
        // ABI Mínimo del Router de Sushi (getAmountsOut)
        string abi = @"[{
            'constant': true,
            'inputs': [{'name': 'amountIn', 'type': 'uint256'}, {'name': 'path', 'type': 'address[]'}],
            'name': 'getAmountsOut',
            'outputs': [{'name': 'amounts', 'type': 'uint256[]'}],
            'type': 'function'
        }]";

        var contract = web3.Eth.GetContract(abi, SUSHI_ROUTER);
        var function = contract.GetFunction("getAmountsOut");

        // Preguntamos: "Si te doy 1 WETH (10^18), ¿cuántos DAI me das?"
        var amountIn = Web3.Convert.ToWei(1); 
        var path = new string[] { WETH, DAI };

        var result = await function.CallAsync<System.Collections.Generic.List<BigInteger>>(amountIn, path);
        
        // El resultado es una lista: [Input, Output]. Queremos el segundo (Output).
        var amountOut = result[1];
        
        // Convertimos de Wei (18 decimales) a Human Readable
        return Web3.Convert.FromWei(amountOut);
    }

    static async Task<decimal> GetUniPrice(Web3 web3)
    {
        // ABI Mínimo del Quoter de Uniswap V3 (quoteExactInputSingle)
        string abi = @"[{
            'inputs': [{'name': 'tokenIn', 'type': 'address'}, {'name': 'tokenOut', 'type': 'address'}, {'name': 'fee', 'type': 'uint24'}, {'name': 'amountIn', 'type': 'uint256'}, {'name': 'sqrtPriceLimitX96', 'type': 'uint160'}],
            'name': 'quoteExactInputSingle',
            'outputs': [{'name': 'amountOut', 'type': 'uint256'}],
            'stateMutability': 'view',
            'type': 'function'
        }]";

        var contract = web3.Eth.GetContract(abi, UNI_QUOTER);
        var function = contract.GetFunction("quoteExactInputSingle");

        // Parámetros para Uniswap V3
        var amountIn = Web3.Convert.ToWei(1);
        uint fee = 3000; // 0.3%
        BigInteger sqrtPriceLimitX96 = 0;

        var amountOut = await function.CallAsync<BigInteger>(WETH, DAI, fee, amountIn, sqrtPriceLimitX96);

        return Web3.Convert.FromWei(amountOut);
    }
}