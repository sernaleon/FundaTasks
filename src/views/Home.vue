<template>
  <v-container>
    <v-alert
      v-if="!$msal.isAuthenticated"
      class="d-flex align-center"
      border="top"
      colored-border
      type="info"
      elevation="2"
    >
      Welcome to Funda Tasks. Sign in to see your profile.
    </v-alert>
    <v-card  v-if="$msal.isAuthenticated" class="mx-auto" elevation="2"  max-width="374">
      <v-card-title>Welcome to Funda Tasks!</v-card-title>
      <v-card-text>
        Tasks will go here once we wire it up to call our API!
      </v-card-text>
      <v-card-actions>
        <v-btn @click="getTasksAsync()">Get tasks</v-btn>
      </v-card-actions>
            <v-card-text v-if="tasks">
              <ul>
                <li v-for="item in tasks" :key="item.id">
                  {{ item.task.name }}: {{ item.description }}
                </li>
              </ul>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import tasksApi, { TaskLineItem } from "../api/tasks";

@Component({
  components: {},
})
export default class Home extends Vue {

   public tasks: TaskLineItem[] = [];
   
   async getTasksAsync() {
     this.tasks = await tasksApi.getTasksAsync();
  }
}
</script>
