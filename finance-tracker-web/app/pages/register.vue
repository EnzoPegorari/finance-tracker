<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '~/components/ui/card'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'

definePageMeta({ layout: 'auth' })

const { register } = useAuth()
const router = useRouter()

const name = ref('')
const email = ref('')
const password = ref('')
const isSubmitting = ref(false)
const errorMessage = ref('')

async function onSubmit() {
  errorMessage.value = ''
  isSubmitting.value = true
  try {
    await register(name.value, email.value, password.value)
    await router.push('/dashboard')
  }
  catch (error: any) {
    errorMessage.value = error?.data?.statusMessage || error?.data?.error || 'Unable to create your account.'
  }
  finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <Card class="glow-border w-full max-w-sm transition-shadow duration-300 hover:shadow-[0_0_0_1px_theme(colors.cyan.400/20%),0_0_50px_-10px_theme(colors.cyan.400/45%)]">
    <CardHeader>
      <CardTitle class="text-2xl">
        Create account
      </CardTitle>
      <CardDescription>Start organizing your finances</CardDescription>
    </CardHeader>
    <CardContent>
      <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <Label for="name">Name</Label>
          <Input id="name" v-model="name" type="text" required autocomplete="name" />
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="email">Email</Label>
          <Input id="email" v-model="email" type="email" placeholder="you@example.com" required autocomplete="email" />
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="password">Password</Label>
          <Input id="password" v-model="password" type="password" minlength="8" required autocomplete="new-password" />
        </div>
        <p v-if="errorMessage" class="text-sm text-destructive">
          {{ errorMessage }}
        </p>
        <Button type="submit" class="w-full" :disabled="isSubmitting">
          {{ isSubmitting ? 'Creating account...' : 'Create account' }}
        </Button>
      </form>
      <p class="mt-4 text-center text-sm text-muted-foreground">
        Already have an account?
        <NuxtLink to="/login" class="text-primary underline-offset-4 hover:underline">
          Sign in
        </NuxtLink>
      </p>
    </CardContent>
  </Card>
</template>
