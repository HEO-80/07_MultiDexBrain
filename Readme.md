<div align="center">

# 🧠 Multi-DEX Brain V2 — Evolved Arbitrage Controller

<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white"/>
<img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/Nethereum-3C3C3D?style=for-the-badge&logo=ethereum&logoColor=white"/>
<img src="https://img.shields.io/badge/Alchemy-363FF9?style=for-the-badge&logo=alchemy&logoColor=white"/>

**Evolution of `06_MultiDexArbitrage` — refined architecture, same battle-tested core**

*Gas estimation → atomic execution → receipt validation. Now with cleaner separation of concerns.*

**🌍 [English](#-english-version) · 🇪🇸 [Español](#-versión-en-español)**

</div>

---

## 🇪🇸 Versión en Español

### 📈 ¿Por qué existe este repositorio?

`07_MultiDexBrain` es la **evolución directa de `06_MultiDexArbitrage`**. Mantiene el mismo núcleo battle-tested — estimación de gas, ejecución síncrona y validación de recibo — pero con una arquitectura más limpia y preparada para escalar hacia versiones más complejas del ecosistema.

> Cada repo de esta serie es un peldaño. El 06 demostró que el ciclo funcionaba. El 07 lo consolida antes de añadir más complejidad.

---

### 🔄 06 vs 07 — ¿Qué cambió?

| Característica | `06_MultiDexArbitrage` | `07_MultiDexBrain` *(este)* |
|:---|:---|:---|
| Núcleo de ejecución | ✅ Funcional | ✅ Refactorizado |
| Separación de responsabilidades | Básica | Mejorada |
| Preparación para escalar | Limitada | ✅ Base para versiones 08+ |
| Estructura del proyecto | Plana | Organizada por módulos |
| Configuración `.env` | ✅ | ✅ |

---

### ⚙️ Flujo de Ejecución
```
Program.cs
    │
    ├── 1. Cargar credenciales (.env)
    │       └── ALCHEMY_URL · PRIVATE_KEY · BOT_ADDRESS
    │
    ├── 2. Estimación de gas previa
    │       └── EstimateGasAsync()
    │           ├── Simula la TX antes de emitirla
    │           └── Evita transacciones fallidas por gas insuficiente
    │
    ├── 3. Ejecución síncrona
    │       └── SendTransactionAndWaitForReceiptAsync()
    │           ├── Construye + firma + emite la TX
    │           └── Espera confirmación del bloque minado
    │
    └── 4. Evaluación del recibo
            ├── Status = 1 ✅ → arbitraje ejecutado con beneficio
            └── Status = 0 ❌ → revert automático → fondos protegidos
```

---

### 🗼 La Torre de Control

> **La Torre *(este código)*** calcula el combustible exacto, da la orden de despegue y espera en la radio hasta confirmar *(Status 1)* o abortar *(Status 0)*.
>
> **El Avión *(el Smart Contract)*** ejecuta el viaje en la blockchain y puede hacer un **abort de emergencia** si los precios cambian en el último milisegundo.

---

### 🛠️ Tech Stack

| Capa | Tecnología |
|:---|:---|
| Lenguaje | C# / .NET 10.0 |
| Web3 Integration | Nethereum |
| Nodo RPC | Alchemy |
| Seguridad | DotNetEnv (.env) |

---

### 🏗️ Estructura del Proyecto
```
07_MultiDexBrain/
├── 07_MultiDexBrain/
│   └── Program.cs              # Controlador principal
├── 07_MultiDexBrain.sln        # Solución .NET
├── .env                        # Credenciales (NO subir a Git)
└── README.md
```

---

### 🚀 Configuración y Ejecución

**1. Crear `.env` en la raíz**
```env
ALCHEMY_URL=https://eth-mainnet.g.alchemy.com/v2/TU_API_KEY
PRIVATE_KEY=TU_CLAVE_PRIVADA
BOT_ADDRESS=0x_DIRECCION_DEL_CONTRATO
```

**2. Ejecutar**
```bash
dotnet run
```

---

### 🔗 Posición en el Ecosistema DeFi

| Fase | Repo | Rol |
|:---:|:---|:---|
| 1 | `Flash_Loans` | ⚡ Contrato Solidity — lógica on-chain |
| 2 | `03_FlashLoanDriver` | 🚀 Driver local — pruebas aisladas |
| 3 | `04_MarketScanner` | 📡 Radar — precios en tiempo real |
| 4 | `05_ArbitrageBot` | 🤖 V1 — primer disparo real |
| 5 | `06_MultiDexArbitrage` | 🧠 Ciclo completo con validación |
| **6** | **`07_MultiDexBrain`** *(este)* | **🔄 V2 — arquitectura refinada y escalable** |
| 7 | `09_ProfitBrain` | 💰 Controlador Mainnet — gestión de riesgo |
| 8 | `10_RealPriceBrain` | 🎯 Cerebro — detección automática de spreads |
| 9 | `13_SniperBot` | 🏹 Sniper — captura tokens nuevos en BSC |

---

### ⚖️ Disclaimer

Este proyecto es **exclusivamente para fines educativos e investigación DeFi**. Los autores no son responsables de pérdidas financieras ni daños derivados del uso de este software.

---

### 🧑‍💻 Autor

**Héctor Oviedo** — Backend Developer & DeFi Researcher

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/hectorob/)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/HEO-80)

---
---

## 🇬🇧 English Version

### 📈 Why does this repository exist?

`07_MultiDexBrain` is the **direct evolution of `06_MultiDexArbitrage`**. It keeps the same battle-tested core — gas estimation, synchronous execution and receipt validation — but with a cleaner architecture ready to scale toward more complex versions of the ecosystem.

> Each repo in this series is a step on the ladder. `06` proved the cycle worked. `07` consolidates it before adding more complexity.

---

### 🔄 06 vs 07 — What changed?

| Feature | `06_MultiDexArbitrage` | `07_MultiDexBrain` *(this)* |
|:---|:---|:---|
| Execution core | ✅ Functional | ✅ Refactored |
| Separation of concerns | Basic | Improved |
| Ready to scale | Limited | ✅ Base for v08+ |
| Project structure | Flat | Organized by modules |
| `.env` config | ✅ | ✅ |

---

### ⚙️ Execution Flow
```
Program.cs
    │
    ├── 1. Load credentials (.env)
    │       └── ALCHEMY_URL · PRIVATE_KEY · BOT_ADDRESS
    │
    ├── 2. Prior gas estimation
    │       └── EstimateGasAsync()
    │           ├── Simulates TX before broadcasting
    │           └── Prevents failed transactions
    │
    ├── 3. Synchronous execution
    │       └── SendTransactionAndWaitForReceiptAsync()
    │           ├── Build + sign + broadcast TX
    │           └── Wait for mined block confirmation
    │
    └── 4. Receipt evaluation
            ├── Status = 1 ✅ → arbitrage executed with profit
            └── Status = 0 ❌ → automatic revert → funds protected
```

---

### 🚀 Setup & Execution

**1. Create `.env` in project root**
```env
ALCHEMY_URL=https://eth-mainnet.g.alchemy.com/v2/YOUR_API_KEY
PRIVATE_KEY=YOUR_PRIVATE_KEY
BOT_ADDRESS=0x_YOUR_CONTRACT_ADDRESS
```

**2. Run**
```bash
dotnet run
```

---

### 🔗 Position in the DeFi Ecosystem

| Phase | Repo | Role |
|:---:|:---|:---|
| 1 | `Flash_Loans` | ⚡ Solidity contract — on-chain logic |
| 2 | `03_FlashLoanDriver` | 🚀 Local driver — isolated testing |
| 3 | `04_MarketScanner` | 📡 Radar — real-time price reading |
| 4 | `05_ArbitrageBot` | 🤖 V1 — first real trigger |
| 5 | `06_MultiDexArbitrage` | 🧠 Full cycle with validation |
| **6** | **`07_MultiDexBrain`** *(this)* | **🔄 V2 — refined, scalable architecture** |
| 7 | `09_ProfitBrain` | 💰 Mainnet controller — risk management |
| 8 | `10_RealPriceBrain` | 🎯 Brain — automatic spread detection |
| 9 | `13_SniperBot` | 🏹 Sniper — captures new tokens on BSC |

---

### ⚖️ Disclaimer

This project is for **educational and DeFi research purposes only**. The authors are not responsible for financial losses or damages from using this software.

---

### 🧑‍💻 Author

**Héctor Oviedo** — Backend Developer & DeFi Researcher

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/hectorob/)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/HEO-80)

---

<div align="center">
  <sub>Built with ☕ and DeFi research · <strong>Héctor Oviedo</strong> · Zaragoza, España</sub>
</div>
