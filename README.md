![Version](https://img.shields.io/badge/version-1.1.0-blue.svg)
![Jeux](https://img.shields.io/badge/Jeux-Nova%20Life-orange.svg)
![License](https://img.shields.io/badge/license-GNU%20GPL%20v3-red.svg)
![GitHub all releases](https://img.shields.io/github/downloads/Robocnop/HueConnectRefactored/total)

# 📗 **HueConnectRefactored** v1.1.0

> **Basé sur le plugin de** : *Zerox_Hue*  
> **Refactorisé par** : *Robocnop*  
> **Objectif** : Faire fonctionner HueConnect avec les versions actuelles de NovaLife et le rendre plus léger en taille.

---

## 🔧 Fonctionnalités

- Envoie un message, **uniquement aux administrateurs**, lorsqu’un joueur se connecte (une seule fois par connexion, pas à chaque respawn).
- Signale en plus la connexion d’un admin avec son niveau.
- Log Discord (embed) de chaque connexion :
  - Nom RP et nom de compte
  - Nom Steam, ID Steam et lien vers le profil Steam
  - Niveau admin si le joueur est admin
- Signale en console quand une nouvelle version est disponible.

---

## ⬇️ Installation

1. **Téléchargez** `HueConnectRefactored.dll` depuis les [Releases](https://github.com/Robocnop/HueConnectRefactored/releases).
2. Installez **ModKit** et **AAMenu** ([téléchargement](https://github.com/emilenkz/NovaLife_ModKit-Releases/releases)) si ce n’est pas déjà fait.
3. **Placez** `HueConnectRefactored.dll` dans le dossier `Plugins` de votre serveur NovaLife.
4. **Lancez** votre serveur, puis **éditez** `Plugins/HueConnectRefactored/config.json` :
   ```json
   {
     "ConfigVersion": "1.1.0",
     "LoginWebhookUrl": "https://discord.com/api/webhooks/…",
     "CheckForUpdates": true
   }
   ```
5. **Redémarrez** le serveur pour appliquer la configuration.

| Option | Par défaut | Description |
|---|---|---|
| `LoginWebhookUrl` | vide | Webhook Discord des connexions (vide = pas de log Discord) |
| `CheckForUpdates` | `true` | `false` = désactive la vérification de mise à jour |
| `ConfigVersion` | | Géré par le plugin, ne pas modifier |

Un ancien `config.json` reste compatible : le webhook est conservé et les nouveaux champs sont ajoutés au démarrage.
