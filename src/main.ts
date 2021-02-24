import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import { MsalPlugin, MsalPluginOptions } from './plugins/msal-plugin';
import axios from 'axios';

Vue.config.productionTip = false;

const options: MsalPluginOptions = {
  clientId: process.env.VUE_APP_MSAL_CLIENT_ID,
  loginAuthority:  process.env.VUE_APP_MSAL_LOGIN_AUTHORITY,
  passwordAuthority: process.env.VUE_APP_MSAL_PASSWORD_RESET_AUTHORITY,
  knownAuthority: process.env.VUE_APP_MSAL_KNOWN_AUTHORITY,
  apiScope: process.env.VUE_APP_MSAL_API_SCOPE
};

axios.defaults.baseURL = process.env.VUE_APP_API_BASE_URL;

Vue.use(new MsalPlugin(), options);

new Vue({
  vuetify,
  render: h => h(App)
}).$mount("#app");
