<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <h1>Funda Tasks</h1>
      </div>

      <v-spacer></v-spacer>

      <button v-if="!isAuthenticated" @click="signIn()">Sign In</button>
      <button v-if="isAuthenticated" @click="signOut()">Sign Out</button>
    </v-app-bar>

    <v-main>
      <Home />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop, Emit } from "vue-property-decorator";
import Home from './views/Home.vue'

@Component({
  components: { Home },
})
export default class App extends Vue {

  public get isAuthenticated(): boolean {
    return this.$msal.isAuthenticated;
  }

  public async signIn() {
    await this.$msal.signIn();
  }

  public async signOut() {
    await this.$msal.signOut();
  }
}
</script>
