<p align="center">
  <img src="https://img.shields.io/badge/API-ASP.NET_Core-1f2937?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Database-SQLite-1f2937?style=for-the-badge&logo=sqlite" />
  <img src="https://img.shields.io/badge/Security-AES--256--GCM-darkslategray?style=for-the-badge&logo=shield" />
  <img src="https://img.shields.io/badge/Auth-JWT-374151?style=for-the-badge&logo=jsonwebtokens" />
</p>

<h1 align="center">CapsuleApi</h1>

<p align="center">
  <em>A secure REST API for storing encrypted data</em><br/>
</p>

---

## Overview

A REST API for storing encrypted secrets with built-in access rules.

Secrets are encrypted using AES-256-GCM and can be configured to expire or self-destruct after a number of reads.

Access is controlled via one-time tokens, and metadata is protected using authenticated encryption (AAD) to prevent tampering.

---


## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Encryption](#encryption)
- [Authentication](#authentication)
- [Optimization](#optimization)
- [Running the Project](#running-the-project)
   


---

# Features

- Encrypted secret storage (optionally with extra rules like set expiration, self destroy upon opening N times)
- Metadata-bound encryption using AAD
- JWT-based authentication

---

## Encryption

Sensitive content is encrypted using **AES-256-GCM**. 

The encrypted output is stored as:

- **Nonce**
- **CipherText**
- **Tag**

### Additional Authenticated Data (AAD)

Metadata is bound to the ciphertext using **AAD (Additional Authenticated Data)**.
AAD is authenticated and verified during decryption. Any modification causes decryption to fail

## Authentication

Users can register and authenticate using secure credentials.

-  Passwords are hashed using **BCrypt**
-  Authentication is handled via **JWT (JSON Web Tokens)**

Protected endpoints require a valid token:

```http
Authorization: Bearer <token>
```

# Architecture
The project follows a layered structure:

---
```text
src/
├── Api/            # Controllers, middleware, startup/config
├── App/            # DTOs, services, interfaces, business logic
├── Domain/         # Core entities
├── Infrastructure/ # DbContext, repositories, persistence
```

 

# Optimization

## Caching results

<details>
<summary><b> Click to view Performance Results (Before vs. After)</b></summary>

Before Caching 

<img width="805" alt="Performance Before Caching" src="https://github.com/user-attachments/assets/fd1851d5-e37a-43e1-bafd-589743638b8d" />

After Caching 

<img width="824" alt="Performance After Caching" src="https://github.com/user-attachments/assets/a6f5d9b1-2a29-4b18-b545-bdb2859e9241" />

</details>


## Running the project

1. Clone the project:
   ```bash
   git clone [https://github.com/AlexanderJson/NET25-K4U1.git](https://github.com/AlexanderJson/NET25-K4U1.git)
   ```

2. In your IDE - navigate to `(myapi/src/api)` to run the api.

3. During the development phase a fresh 32-byte key will be generated and destroyed by each run.

4. To get a JwT token - user/post

5. To encrypt/decrypt use the `secret/` endpoint.




