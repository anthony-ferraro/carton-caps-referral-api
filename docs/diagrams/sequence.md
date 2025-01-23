sequenceDiagram
    participant R as Referrer App
    participant A as API
    participant N as New User App

    R->>A: GET /v1/referrals/my-code
    A->>R: Return referral code

    R->>A: POST /v1/referrals/generate-link/{shareMethod}
    A->>R: Return shareable link

    R->>N: Share link via platform (text/email/share)

    N->>A: GET /v1/referrals/validate/{code}
    A->>N: Return validation result

    Note over N,A: User completes signup via separate API...
    R->>A: GET /v1/referrals/history
    A->>R: Return referral list

    Note over R,A: All error responses include:
    Note over R,A: - Error code
    Note over R,A: - Error message
