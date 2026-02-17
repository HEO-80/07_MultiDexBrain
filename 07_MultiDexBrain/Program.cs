using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using DotNetEnv;
class Program
{
    // --- CONFIGURACIÓN DE LA FUSIÓN ---
    
    // 1. CONEXIÓN: Usamos TU Anvil local (No Alchemy directo) para poder ejecutar
    static string anvilUrl = "http://127.0.0.1:8545";
    
    // 2. CUENTA: La clave privada #0 de Anvil (El millonario de la simulación)
    static string privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";

    // 3. OBJETIVO: La dirección de TU contrato desplegado (Sacada de tu captura)
    static string botAddress = "0xE5D42D6E49e8d2C2089D769Ef10D35f45a3c1cCb";

    // Direcciones de Monedas
    static string DAI = "0x6B175474E89094C44Da98b954EedeAC495271d0F";

    static async Task Main(string[] args)
    {

        // 1. CARGAMOS EL ARCHIVO SECRETO
        Env.Load();
        
        // 2. LEEMOS LA CLAVE (Solo si la necesitamos para conectar a Mainnet)
        string alchemyKey = Environment.GetEnvironmentVariable("ALCHEMY_KEY");
        
        Console.WriteLine("🔌 CONECTANDO CEREBRO A CUERPO (ANVIL)...");
        
        // Creamos la cuenta con la clave privada para poder firmar transacciones
        var account = new Account(privateKey, 31337); // 31337 es el ChainId de Anvil
        var web3 = new Web3(account, anvilUrl);

        try
        {
            var netVersion = await web3.Net.Version.SendRequestAsync();
            Console.WriteLine($"✅ Conectado a Red Local (ChainID: {netVersion})");
            Console.WriteLine($"🤖 Objetivo fijado: {botAddress}");
            Console.WriteLine("---------------------------------------------");

            while (true)
            {
                Console.WriteLine("\nPresiona [ENTER] para DISPARAR el Arbitraje (o 'Ctrl+C' para salir)");
                Console.ReadLine(); // Espera a que des Enter

                Console.WriteLine("🔫 ¡FUEGO! Iniciando Flash Loan...");

                // 1. Definimos la función que vamos a llamar (ABI)
                string abi = @"[{
                    'inputs': [{'internalType': 'address', 'name': 'token', 'type': 'address'}, {'internalType': 'uint256', 'name': 'cantidad', 'type': 'uint256'}],
                    'name': 'iniciarArbitraje',
                    'outputs': [],
                    'stateMutability': 'nonpayable',
                    'type': 'function'
                }]";

                var contract = web3.Eth.GetContract(abi, botAddress);
                var funcionArbitraje = contract.GetFunction("iniciarArbitraje");

                // 2. Preparamos el disparo (1,000 DAI)
                var cantidad = Web3.Convert.ToWei(1000); // 1000 DAI
                
                // 3. Estimamos Gas y Enviamos
                try 
                {
                    var gas = await funcionArbitraje.EstimateGasAsync(botAddress, null, null, DAI, cantidad);
                    Console.WriteLine($"⛽ Gas Estimado: {gas}");

                    var receipt = await funcionArbitraje.SendTransactionAndWaitForReceiptAsync(botAddress, gas, null, null, DAI, cantidad);
                    
                    Console.WriteLine("---------------------------------------------");
                    if (receipt.Status.Value == 1)
                    {
                         Console.ForegroundColor = ConsoleColor.Green;
                         Console.WriteLine($"🏆 ÉXITO: Transacción Confirmada! Hash: {receipt.TransactionHash}");
                    }
                    else
                    {
                         Console.ForegroundColor = ConsoleColor.Red;
                         Console.WriteLine($"💀 FALLO: La transacción revirtió (Posiblemente falta de fondos para repagar).");
                         Console.WriteLine($"Hash: {receipt.TransactionHash}");
                    }
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ ERROR AL EJECUTAR: {ex.Message}");
                    Console.WriteLine("NOTA: Es normal que falle si el bot no tiene saldo extra para cubrir las pérdidas del arbitraje.");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR DE CONEXIÓN: {ex.Message}");
        }
    }
}