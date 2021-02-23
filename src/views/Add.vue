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
      <v-card-title>Add task</v-card-title>
      <v-text-field v-model="task.name"></v-text-field>
      <v-text-field v-model="task.description"></v-text-field>
      <v-card-actions>
        <v-btn @click="addTask()">Add</v-btn>
      </v-card-actions>
            <v-card-text v-if="tasks">
          {{tasks}}
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import tasksApi, { NewTask, TaskLineItem } from "../api/tasks";

@Component({
  components: {},
})
export default class Add extends Vue {

   public task: NewTask = {
     name: "",
     description: ""
   };
   
   async addTask() {
     const results = await tasksApi.addTaskAsync(this.task);
     console.log(results);
  }
}
</script>
