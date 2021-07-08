export class AuthenticationService {
    constructor(router, axios) {
        this.router = router;
        this.axios = axios;
    }
    async isAuthenticatedAsync() {
        return await this.axios.get('/account/user');
    }

    async authenticateAsync(returnUrl) {
        if (this.router.history.current.path.name !== 'Login') {
            this.router.push({
                name: 'Login',
                params: {
                    returnUrl: returnUrl
                }
            });
        }
    }

    login(returnUrl) {
        this.router.push(`/api/account/login?returnUrl=${returnUrl}`, () => {
            this.router.go();
        });
    }
}
